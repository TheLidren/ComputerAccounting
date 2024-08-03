using ClosedXML.Excel;
using ComputerAccounting.App_Data;
using ComputerAccounting.App_Start;
using ComputerAccounting.ListViewModel;
using ComputerAccounting.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace ComputerAccounting.Controllers
{
    public class EmployeeController : Controller
    {
        CompContent db = new();
        static readonly Regex trimmer = new(@"\s\s+");

        [HttpGet]
        public ActionResult ShowChart()
        {
            LineChartModel line = new();
            line.OnEmployee();
            return View(line);
        }

        public ActionResult ExportEmployee()
        {   
            List<Employee> employees = db.Employees.Include(p => p.Position).Where(m => m.Status).ToList();
            XLWorkbook workbook = new();
            var worksheet = workbook.Worksheets.Add("Работники");
            worksheet.Cell("A1").Value = "ФИО";
            worksheet.Cell("B1").Value = "Номер телефона";
            worksheet.Cell("C1").Value = "Дата трудоустройства";
            worksheet.Cell("D1").Value = "Должность";
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Range($"A1:D{employees.Count + 1}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{employees.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
            worksheet.Range($"A1:D{employees.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{employees.Count + 1}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{employees.Count + 1}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{employees.Count + 1}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            for (int i = 0; i < employees.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = employees[i].Surname + " " + employees[i].Name + " " + employees[i].Patronymic;
                worksheet.Cell(i + 2, 2).Value = employees[i].PhoneNumber;
                worksheet.Cell(i + 2, 3).Value = employees[i].DateWork.ToString("d");
                worksheet.Cell(i + 2, 4).Value = employees[i].Position.Tittle;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Flush();
            return new FileContentResult(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"Работники_{DateTime.UtcNow.ToShortDateString()}.xlsx"
            };
        }

        public ActionResult ExportWord(int? employeeid)
        {
            Employee employee = db.Employees.Find(employeeid);
            if (employeeid == null)
                return HttpNotFound();
            string path = @$"C:\Users\Владислав Мисевич\Downloads\Трудовой договор {employee.Surname} {employee.Name}.docx";
            FileInfo fileInf = new(@$"{AppContext.BaseDirectory}Samples\EmployeeSample.docx");
            FileInfo fileInf2 = new(path);
            if (fileInf2.Exists)
                fileInf2.Delete();
            if (fileInf.Exists)
                fileInf.CopyTo(path, true);
            Microsoft.Office.Interop.Word.Application wordApp = new();
            wordApp.Visible = false;
            Microsoft.Office.Interop.Word.Document wordDocument = wordApp.Documents.Open(path);
            Position position = db.Positions.Find(employee.PositionId);
            ShowWord.ReplaceWordStub("<Id>", employee.Id.ToString(), wordDocument);
            ShowWord.ReplaceWordStub("<DateWork>", employee.DateWork.ToString("d"), wordDocument);
            ShowWord.ReplaceWordStub("<Employee>", employee.Surname + " " + employee.Name + " " + employee.Patronymic, wordDocument);
            ShowWord.ReplaceWordStub("<Position>", position.Tittle, wordDocument);
            ShowWord.ReplaceWordStub("<Employee>", employee.Surname + " " + employee.Name + " " + employee.Patronymic, wordDocument);
            ShowWord.ReplaceWordStub("<DateWork>", employee.DateWork.ToString("d"), wordDocument);
            ShowWord.ReplaceWordStub("<PhoneNumber>", employee.PhoneNumber, wordDocument);
            ShowWord.ReplaceWordStub("<Employee>", employee.Surname + " " + employee.Name + " " + employee.Patronymic, wordDocument);
            wordApp.Visible = true;
            return RedirectToAction("ListEmployee");
        }


        [HttpGet]
        public ActionResult ListEmployee(int? position, string param, SortState sortOrder = SortState.SurnameAsc)
        {
            IQueryable<Employee> employees = db.Employees.Include(p => p.Position).Where(m => m.Status);
            if (position != null && position != 0)
                employees = employees.Where(p => p.PositionId == position);
            if (!String.IsNullOrEmpty(param))
            {
                param = trimmer.Replace(param, " ").Trim();
                employees = employees.Where(p => p.Name.Contains(param) || p.Surname.Contains(param) || p.Patronymic.Contains(param) || p.PhoneNumber.Contains(param));
            }
            employees = sortOrder switch
            {
                SortState.SurnameDesc => employees.OrderByDescending(s => s.Surname),
                SortState.DateWorkDesc => employees.OrderByDescending(s => s.DateWork),
                SortState.DateWorkAsc => employees.OrderBy(s => s.DateWork),
                SortState.PositionAsc => employees.OrderBy(s => s.Position.Tittle),
                SortState.PositionDesc => employees.OrderByDescending(s => s.Position.Tittle),
                _ => employees.OrderBy(s => s.Surname),
            };
            EmployeeViewModel viewModel = new()
            {
                SortViewModel = new EmployeeSort(sortOrder),
                FilterViewModel = new EmployeeFilter(db.Positions.ToList(), position, param),
                Employees = employees.ToList()
            };
            ViewBag.param = param;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult CreateEmployee()
        {
            SelectList positions = new(db.Positions, "Id", "Tittle");
            ViewBag.positions = positions;
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmployee(Employee employee)
        {
            SelectList positions = new(db.Positions, "Id", "Tittle", employee.PositionId);
            ViewBag.positions = positions;
            if (ModelState.IsValid)
            {
                if (employee.DateWork.Year < 2020 || employee.DateWork.Date > System.DateTime.Today)
                {
                    ModelState.AddModelError("DateWork", "Укажите корректно дату. Min = 01.01.21 и Max = today");
                    return View(employee);
                }
                employee.Status = true;
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("ListEmployee");
            }
            return View(employee);
        }

        [HttpGet]
        public ActionResult EditEmployee(int? employeeid)
        {
            Employee employee = db.Employees.Find(employeeid);
            if (employeeid == null)
                return HttpNotFound();
            SelectList positions = new(db.Positions, "Id", "Tittle", employee.PositionId);
            ViewBag.positions = positions;
            return View(employee);
        }

        [HttpPost]
        public ActionResult EditEmployee(Employee employee)
        {
            SelectList positions = new(db.Positions, "Id", "Tittle", employee.PositionId);
            ViewBag.positions = positions;
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListEmployee");
            }
            return View(employee);
        }

        [HttpGet]
        public ActionResult DeleteEmployee(int? employeeid)
        {
            Employee employee = db.Employees.Find(employeeid);
            if (employeeid == null)
                return HttpNotFound();
            employee.Status = false;
            db.SaveChanges();
            return RedirectToAction("ListEmployee");
        }
    }
}
