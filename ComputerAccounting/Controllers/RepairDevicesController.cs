using ComputerAccounting.App_Data;
using ComputerAccounting.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Data.Entity;
using ComputerAccounting.ListViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using ComputerAccounting.App_Start;
using ClosedXML.Excel;

namespace ComputerAccounting.Controllers
{
    public class RepairDevicesController : Controller
    {
        CompContent db = new();
        static readonly Regex trimmer = new(@"\s\s+");

        [HttpGet]
        public ActionResult ShowChart()
        {
            return View(db.RepairDevices.Include(m => m.CatalogParts).Include(m => m.Device).Where(m => m.Status));
        }
        
        public ActionResult ExportRepairDevices()
        {
            List<RepairDevices> repairDevices = db.RepairDevices.Include(m => m.CatalogParts).Include(m => m.Device).Where(m => m.Status).ToList();
            XLWorkbook workbook = new();
            var worksheet = workbook.Worksheets.Add("Ремонт устройств");
            worksheet.Cell("A1").Value = "Устройство";
            worksheet.Cell("B1").Value = "Сломанная деталь";
            worksheet.Cell("C1").Value = "Замененная деталь";
            worksheet.Cell("D1").Value = "Дата ремонта";
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Range($"A1:D{repairDevices.Count + 1}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{repairDevices.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
            worksheet.Range($"A1:D{repairDevices.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{repairDevices.Count + 1}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{repairDevices.Count + 1}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{repairDevices.Count + 1}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            for (int i = 0; i < repairDevices.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = repairDevices[i].Device.Tittle;
                worksheet.Cell(i + 2, 2).Value = repairDevices[i].BrokenParts;
                worksheet.Cell(i + 2, 3).Value = repairDevices[i].CatalogParts.Tittle;
                worksheet.Cell(i + 2, 4).Value = repairDevices[i].DateRepair.ToString("d");
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Flush();
            return new FileContentResult(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"Ремонт устройств_{DateTime.UtcNow.ToShortDateString()}.xlsx"
            };
        }

        public ActionResult ExportWord(int? repairDevicesid)
        {
            RepairDevices repairDevices = db.RepairDevices.Find(repairDevicesid);
            Device device = db.Devices.Find(repairDevices.DeviceId);
            TypeDevice typeDevice = db.TypeDevices.Find(device.TypeDeviceId);
            Provider provider = db.Providers.Find(device.ProviderId);
            CatalogParts catalogParts = db.CatalogParts.Find(repairDevices.CatalogPartsId);
            if (repairDevicesid == null)
                return HttpNotFound();
            string path = @$"C:\Users\Владислав Мисевич\Downloads\ Ремонт устройства {device.Tittle}.docx";
            FileInfo fileInf = new(@$"{AppContext.BaseDirectory}Samples\RepairDevicesSample.docx");
            FileInfo fileInf2 = new(path);
            if (fileInf2.Exists)
                fileInf2.Delete();
            if (fileInf.Exists)
                fileInf.CopyTo(path, true);
            Microsoft.Office.Interop.Word.Application wordApp = new();
            wordApp.Visible = false;
            Microsoft.Office.Interop.Word.Document wordDocument = wordApp.Documents.Open(path);
            ShowWord.ReplaceWordStub("<Id>", repairDevices.Id.ToString(), wordDocument);
            ShowWord.ReplaceWordStub("<DateRepair>", repairDevices.DateRepair.ToString("d"), wordDocument);
            ShowWord.ReplaceWordStub("<Tittle>", device.Tittle, wordDocument);
            ShowWord.ReplaceWordStub("<TypeDevice>", typeDevice.Tittle, wordDocument);
            ShowWord.ReplaceWordStub("<BrokenParts>", repairDevices.BrokenParts, wordDocument);
            ShowWord.ReplaceWordStub("<TittleParts>", catalogParts.Tittle, wordDocument);
            ShowWord.ReplaceWordStub("<Price>", catalogParts.Price.ToString(), wordDocument);
            ShowWord.ReplaceWordStub("<DateRepair>", repairDevices.DateRepair.ToString("d"), wordDocument);
            ShowWord.ReplaceWordStub("<DateBuy>", device.DateBuy.ToString("d"), wordDocument);
            ShowWord.ReplaceWordStub("<Provider>", provider.Surname + " " + provider.Name + " " + provider.Patronymic, wordDocument);
            wordApp.Visible = true;
            return RedirectToAction("ListRepairDevices");
        }

        [HttpGet]
        public ActionResult ListRepairDevices(int? device, int? catalogpart, DateTime? daterepair, RepairDevicesState repairDevicesState = RepairDevicesState.DeviceAsc)
        {
            IQueryable<RepairDevices> repairDevices = db.RepairDevices.Include(m => m.CatalogParts).Include(m => m.Device).Where(m => m.Status);
            if (device != null && device != 0)
                repairDevices = repairDevices.Where(p => p.DeviceId == device);
            if (catalogpart != null && catalogpart != 0)
                repairDevices = repairDevices.Where(p => p.CatalogPartsId == catalogpart);
            if (daterepair != null)
                repairDevices = repairDevices.Where(p => p.DateRepair >= daterepair);
            repairDevices = repairDevicesState switch
            {
                RepairDevicesState.DeviceDesc => repairDevices.OrderByDescending(s => s.Device.Tittle),
                RepairDevicesState.CatalogPartsDesc => repairDevices.OrderByDescending(s => s.CatalogParts.Tittle),
                RepairDevicesState.CatalogPartsAsc => repairDevices.OrderBy(s => s.CatalogParts.Tittle),
                RepairDevicesState.DateRepairAsc => repairDevices.OrderBy(s => s.DateRepair),
                RepairDevicesState.DateRepairDesc => repairDevices.OrderByDescending(s => s.DateRepair),
                _ => repairDevices.OrderBy(s => s.Device.Tittle),
            };
            RepairDevicesViewModel viewModel = new()
            {
                RepairDevices = repairDevices.ToList(),
                RepairDevicesSort = new RepairDevicesSort(repairDevicesState),
                RepairDevicesFilter = new RepairDevicesFilter(db.Devices.ToList(), device, db.CatalogParts.ToList(), catalogpart, daterepair),
            };
            ViewBag.daterepair = daterepair;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult CreateRepairDevices()
        {
            SelectList devices = new(db.Devices, "Id", "Tittle");
            ViewBag.devices = devices;
            SelectList catalogparts = new(db.CatalogParts, "Id", "Tittle");
            ViewBag.catalogparts = catalogparts;
            return View();
        }

        [HttpPost]
        public ActionResult CreateRepairDevices(RepairDevices repairDevices)
        {
            SelectList devices = new(db.Devices, "Id", "Tittle", repairDevices.DeviceId);
            ViewBag.devices = devices;
            SelectList catalogparts = new(db.CatalogParts, "Id", "Tittle", repairDevices.CatalogPartsId);
            ViewBag.catalogparts = catalogparts;
            if (ModelState.IsValid)
            {
                Device device = db.Devices.Find(repairDevices.DeviceId);
                if (repairDevices.DateRepair.Date < device.DateBuy.Date || repairDevices.DateRepair.Date > DateTime.Today)
                {
                    ModelState.AddModelError("DateRepair", $"Укажите корректно дату. Min = {device.DateBuy:d} и Max = {DateTime.Today:d}");
                    return View(repairDevices);
                }
                repairDevices.BrokenParts = trimmer.Replace(repairDevices.BrokenParts, " ").Trim();
                repairDevices.Status = true;
                CatalogParts catalog = db.CatalogParts.Find(repairDevices.CatalogPartsId);
                if (catalog.Count > 0)
                    catalog.Count--;
                else catalog.Status = false;
                db.RepairDevices.Add(repairDevices);
                db.SaveChanges();
                return RedirectToAction("ListRepairDevices");
            }
            return View(repairDevices);
        }

        [HttpGet]
        public ActionResult EditRepairDevices(int? repairDevicesid)
        {
            RepairDevices repairDevices = db.RepairDevices.Find(repairDevicesid);
            if (repairDevicesid == null)
                return HttpNotFound();
            SelectList devices = new(db.Devices, "Id", "Tittle", repairDevices.DeviceId);
            ViewBag.devices = devices;
            SelectList catalogparts = new(db.CatalogParts, "Id", "Tittle", repairDevices.CatalogPartsId);
            ViewBag.catalogparts = catalogparts;
            return View(repairDevices);
        }

        [HttpPost]
        public ActionResult EditRepairDevices(RepairDevices repair)
        {
            if (ModelState.IsValid)
            {
                repair.BrokenParts = trimmer.Replace(repair.BrokenParts, " ").Trim();
                db.Entry(repair).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListRepairDevices");
            }
            return View(repair);
        }

        [HttpGet]
        public ActionResult DeleteRepairDevices(int? repairDevicesid)
        {
            RepairDevices typedevice = db.RepairDevices.Find(repairDevicesid);
            if (repairDevicesid == null)
                return HttpNotFound();
            typedevice.Status = false;
            db.SaveChanges();
            return RedirectToAction("ListRepairDevices");
        }
    }
}
