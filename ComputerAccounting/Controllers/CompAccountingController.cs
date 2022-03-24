using ComputerAccounting.App_Data;
using ComputerAccounting.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Data.Entity;
using System;
using ComputerAccounting.ListViewModel;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.IO;
using ComputerAccounting.App_Start;

namespace ComputerAccounting.Controllers
{
    public class CompAccountingController : Controller
    {
        CompContent db = new();
        static readonly Regex trimmer = new(@"\s\s+");

        [HttpGet]
        public ActionResult ShowChart()
        {
            LineChartModel line = new();
            line.OnGet();
            return View(line);
        }

        public ActionResult ExportCompAccounting()
        {
            List<CompAccounting> compAccountings = db.CompAccountings.Include(m => m.Device).Include(m => m.Employee).Where(m => m.Status).ToList();
            using XLWorkbook workbook = new(XLEventTracking.Disabled);
            var worksheet = workbook.Worksheets.Add("КомпУчёт");
            worksheet.Cell("A1").Value = "Работник";
            worksheet.Cell("B1").Value = "Устройство";
            worksheet.Cell("C1").Value = "Место расположения";
            worksheet.Cell("D1").Value = "Дата получения";
            worksheet.Cell("E1").Value = "Дата списывания";
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Range($"A1:E{compAccountings.Count + 1}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:E{compAccountings.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
            worksheet.Range($"A1:E{compAccountings.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:E{compAccountings.Count + 1}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:E{compAccountings.Count + 1}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:E{compAccountings.Count + 1}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            for (int i = 0; i < compAccountings.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = compAccountings[i].Employee.Surname + " " + compAccountings[i].Employee.Name + " " + compAccountings[i].Employee.Patronymic;
                worksheet.Cell(i + 2, 2).Value = compAccountings[i].Device.Tittle;
                worksheet.Cell(i + 2, 3).Value = compAccountings[i].PlaceLocated;
                worksheet.Cell(i + 2, 4).Value = compAccountings[i].DateRecieve.ToString("d");
                worksheet.Cell(i + 2, 5).Value = compAccountings[i].DateDelete.ToString("d");
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Flush();
            return new FileContentResult(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"КомпУчёт_{DateTime.UtcNow.ToShortDateString()}.xlsx"
            };
        }

        public ActionResult ExportWord(int? compAccountingid)
        {
            CompAccounting compAccounting = db.CompAccountings.Find(compAccountingid);
            Employee employee = db.Employees.Find(compAccounting.EmployeeId);
            Device device = db.Devices.Find(compAccounting.DeviceId);
            TypeDevice typeDevice = db.TypeDevices.Find(device.TypeDeviceId);
            if (compAccountingid == null)
                return HttpNotFound();
            string path = @$"C:\Users\Владислав Мисевич\Downloads\Компьютерный учёт. {employee.Surname} {employee.Name}.docx";
            FileInfo fileInf = new(@$"{AppContext.BaseDirectory}Samples\CompAccountingSample.docx");
            FileInfo fileInf2 = new(path);
            if (fileInf2.Exists)
                fileInf2.Delete();
            if (fileInf.Exists)
                fileInf.CopyTo(path, true);
            Microsoft.Office.Interop.Word.Application wordApp = new();
            wordApp.Visible = false;
            Microsoft.Office.Interop.Word.Document wordDocument = wordApp.Documents.Open(path);
            ShowWord.ReplaceWordStub("<Id>", compAccounting.Id.ToString(), wordDocument);
            ShowWord.ReplaceWordStub("<DateRecieve>", compAccounting.DateRecieve.ToString("d"), wordDocument);
            ShowWord.ReplaceWordStub("<Employee>", employee.Surname + " " + employee.Name + " " + employee.Patronymic, wordDocument);
            ShowWord.ReplaceWordStub("<Tittle>", device.Tittle, wordDocument);
            ShowWord.ReplaceWordStub("<TypeDevice>", typeDevice.Tittle, wordDocument);
            ShowWord.ReplaceWordStub("<PlaceLocated>", compAccounting.PlaceLocated, wordDocument);
            ShowWord.ReplaceWordStub("<DateRecieve>", compAccounting.DateRecieve.ToString("d"), wordDocument);
            ShowWord.ReplaceWordStub("<DateDelete>", compAccounting.DateDelete.ToString("d"), wordDocument);
            ShowWord.ReplaceWordStub("<Employee>", employee.Surname + " " + employee.Name + " " + employee.Patronymic, wordDocument);
            wordApp.Visible = true;
            return RedirectToAction("ListCompAccounting");
        }


        [HttpGet]
        public ActionResult ListCompAccounting(int? device, int? employee, DateTime? daterec, CompAccountingState compAccountingState = CompAccountingState.DeviceAsc)
        {
            IQueryable<CompAccounting> compAccountings = db.CompAccountings.Include(m => m.Device).Include(m => m.Employee).Where(m => m.Status);
            if (device != null && device != 0)
                compAccountings = compAccountings.Where(p => p.DeviceId == device);
            if (employee != null && employee != 0)
                compAccountings = compAccountings.Where(p => p.EmployeeId == employee);
            if (daterec != null)
                compAccountings = compAccountings.Where(p => p.DateRecieve >= daterec);
            compAccountings = compAccountingState switch
            {
                CompAccountingState.DeviceDesc => compAccountings.OrderByDescending(s => s.Device.Tittle),
                CompAccountingState.EmployeeDesc => compAccountings.OrderByDescending(s => s.Employee.Surname),
                CompAccountingState.EmployeeAsc => compAccountings.OrderBy(s => s.Employee.Surname),
                CompAccountingState.DateReceiveAsc => compAccountings.OrderBy(s => s.DateRecieve),
                CompAccountingState.DateReceiveDesc => compAccountings.OrderByDescending(s => s.DateRecieve),
                CompAccountingState.DateDeleteAsc => compAccountings.OrderBy(s => s.DateDelete),
                CompAccountingState.DateDeleteDesc => compAccountings.OrderByDescending(s => s.DateDelete),
                _ => compAccountings.OrderBy(s => s.Device.Tittle),
            };
            CompAccountingViewModel viewModel = new()
            {
                CompAccountings = compAccountings.ToList(),
                CompAccountingSort = new CompAccountingSort(compAccountingState),
                CompAccountingFilter = new CompAccountingFilter(db.Employees.ToList(), employee, db.Devices.ToList(), device, daterec)
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult CreateCompAccounting()
        {
            SelectList devices = new(db.Devices, "Id", "Tittle");
            ViewBag.devices = devices;
            SelectList employee = new(db.Employees, "Id", "Surname");
            ViewBag.employee = employee;
            return View();
        }

        [HttpPost]
        public ActionResult CreateCompAccounting(CompAccounting compAccountings)
        {
            SelectList devices = new(db.Devices, "Id", "Tittle", compAccountings.DeviceId);
            ViewBag.devices = devices;
            SelectList employee = new(db.Employees, "Id", "Surname", compAccountings.EmployeeId);
            ViewBag.employee = employee;
            if (ModelState.IsValid)
            {
                Device device = db.Devices.Find(compAccountings.DeviceId);
                if (compAccountings.DateRecieve.Date < device.DateBuy.Date || compAccountings.DateRecieve.Date > DateTime.Today)
                {
                    ModelState.AddModelError("DateRecieve", $"Укажите корректно дату. Min = {device.DateBuy:d} и Max = {DateTime.Today:d}");
                    return View(compAccountings);
                }
                if (compAccountings.DateDelete.Date < compAccountings.DateRecieve.Date || compAccountings.DateDelete.Year > DateTime.Today.Year + 3)
                {
                    ModelState.AddModelError("DateDelete", "Дата получения должна быть < Даты списывания");
                    return View(compAccountings);
                }
                compAccountings.PlaceLocated = trimmer.Replace(compAccountings.PlaceLocated, " ").Trim();
                compAccountings.Status = true;
                db.CompAccountings.Add(compAccountings);
                db.SaveChanges();
                return RedirectToAction("ListCompAccounting");
            }
            return View(compAccountings);
        }

        [HttpGet]
        public ActionResult EditCompAccounting(int? compAccountingid)
        {
            CompAccounting compAccountings = db.CompAccountings.Find(compAccountingid);
            if (compAccountingid == null)
                return HttpNotFound();
            SelectList devices = new(db.Devices, "Id", "Tittle", compAccountings.DeviceId);
            ViewBag.devices = devices;
            SelectList employee = new(db.Employees, "Id", "Surname", compAccountings.EmployeeId);
            ViewBag.employee = employee;
            return View(compAccountings);
        }

        [HttpPost]
        public ActionResult EditCompAccounting(CompAccounting acc)
        {
            SelectList devices = new(db.Devices, "Id", "Tittle", acc.DeviceId);
            ViewBag.devices = devices;
            SelectList employee = new(db.Employees, "Id", "Surname", acc.EmployeeId);
            ViewBag.employee = employee;
            if (ModelState.IsValid)
            {
                Device device = db.Devices.Find(acc.DeviceId);
                if (acc.DateRecieve.Date < device.DateBuy.Date || acc.DateRecieve.Date > DateTime.Today)
                {
                    ModelState.AddModelError("DateRecieve", $"Укажите корректно дату. Min = {device.DateBuy:d} и Max = {DateTime.Today:d}");
                    return View(acc);
                }
                if (acc.DateDelete.Date < acc.DateRecieve.Date || acc.DateDelete.Year > DateTime.Today.Year + 3)
                {
                    ModelState.AddModelError("DateDelete", "Дата получения должна быть < Даты списывания");
                    return View(acc);
                }
                acc.PlaceLocated = trimmer.Replace(acc.PlaceLocated, " ").Trim();
                db.Entry(acc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListCompAccounting");
            }
            return View(acc);
        }

        [HttpGet]
        public ActionResult DeleteCompAccounting(int? compAccountingid)
        {
            CompAccounting typedevice = db.CompAccountings.Find(compAccountingid);
            if (compAccountingid == null)
                return HttpNotFound();
            typedevice.Status = false;
            db.SaveChanges();
            return RedirectToAction("ListCompAccounting");
        }
    }
}
