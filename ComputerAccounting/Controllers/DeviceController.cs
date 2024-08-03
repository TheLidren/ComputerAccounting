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
    public class DeviceController : Controller
    {
        CompContent db = new();
        static readonly Regex trimmer = new(@"\s\s+");

        public ActionResult ExportDevice()
        {
            List<Device> devices = db.Devices.Include(p => p.TypeDevice).Include(p => p.Provider).Where(m => m.Status).ToList();
            XLWorkbook workbook = new();
            var worksheet = workbook.Worksheets.Add("Устройства");
            worksheet.Cell("A1").Value = "Устройство";
            worksheet.Cell("B1").Value = "Тип";
            worksheet.Cell("C1").Value = "Поставщик";
            worksheet.Cell("D1").Value = "Организация";
            worksheet.Cell("E1").Value = "Характеристика";
            worksheet.Cell("F1").Value = "Дата покупки";
            worksheet.Cell("G1").Value = "Цена";
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Range($"A1:G{devices.Count + 1}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:G{devices.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
            worksheet.Range($"A1:G{devices.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:G{devices.Count + 1}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:G{devices.Count + 1}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:G{devices.Count + 1}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            for (int i = 0; i < devices.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = devices[i].Tittle;
                worksheet.Cell(i + 2, 2).Value = devices[i].TypeDevice.Tittle;
                worksheet.Cell(i + 2, 3).Value = devices[i].Provider.Surname + " " + devices[i].Provider.Name + " " + devices[i].Provider.Patronymic;
                worksheet.Cell(i + 2, 4).Value = devices[i].Provider.Organization;
                worksheet.Cell(i + 2, 5).Value = devices[i].Characteristics;
                worksheet.Cell(i + 2, 6).Value = devices[i].DateBuy.ToString("d");
                worksheet.Cell(i + 2, 7).Value = devices[i].Price + " руб";
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Flush();
            return new FileContentResult(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"Устройства_{DateTime.UtcNow.ToShortDateString()}.xlsx"
            };
        }

        public ActionResult ExportWord(int? deviceid)
        {
            Device device = db.Devices.Find(deviceid);
            Provider provider = db.Providers.Find(device.ProviderId);
            TypeDevice typeDevice = db.TypeDevices.Find(device.TypeDeviceId);
            if (deviceid == null)
                return HttpNotFound();
            string path = @$"C:\Users\Владислав Мисевич\Downloads\Покупка устройства {device.Tittle}.docx";
            FileInfo fileInf = new(@$"{AppContext.BaseDirectory}Samples\DeviceSample.docx");
            FileInfo fileInf2 = new(path);
            if (fileInf2.Exists)
                fileInf2.Delete();
            if (fileInf.Exists)
                fileInf.CopyTo(path, true);
            Microsoft.Office.Interop.Word.Application wordApp = new();
            wordApp.Visible = false;
            Microsoft.Office.Interop.Word.Document wordDocument = wordApp.Documents.Open(path);
            ShowWord.ReplaceWordStub("<Id>", device.Id.ToString(), wordDocument);
            ShowWord.ReplaceWordStub("<DateBuy>", device.DateBuy.ToString("d"), wordDocument);
            ShowWord.ReplaceWordStub("<Provider>", provider.Surname + " " + provider.Name + " " + provider.Patronymic, wordDocument);
            ShowWord.ReplaceWordStub("<Organization>", provider.Organization, wordDocument);
            ShowWord.ReplaceWordStub("<PhoneNumber>", provider.PhoneNumber, wordDocument);
            ShowWord.ReplaceWordStub("<OrganizationAdress>", provider.OrganizationAdress, wordDocument);
            ShowWord.ReplaceWordStub("<Tittle>", device.Tittle, wordDocument);
            ShowWord.ReplaceWordStub("<TypeDevice>", typeDevice.Tittle, wordDocument);
            ShowWord.ReplaceWordStub("<Characteristics>", device.Characteristics, wordDocument);
            ShowWord.ReplaceWordStub("<DateBuy>", device.DateBuy.ToString("d"), wordDocument);
            ShowWord.ReplaceWordStub("<Price>", device.Price.ToString(), wordDocument);
            ShowWord.ReplaceWordStub("<Provider>", provider.Surname + " " + provider.Name + " " + provider.Patronymic, wordDocument);
            wordApp.Visible = true;
            return RedirectToAction("ListDevice");
        }

        [HttpGet]
        public ActionResult ListDevice(int? typedevice, int? provider, string param, DeviceState deviceState = DeviceState.TittleAsc)
        {
            IQueryable<Device> devices = db.Devices.Include(p => p.TypeDevice).Include(p => p.Provider).Where(m => m.Status);
            if (typedevice != null && typedevice != 0)
                devices = devices.Where(p => p.TypeDeviceId == typedevice);
            if (provider != null && provider != 0)
                devices = devices.Where(p => p.ProviderId == provider);
            if (!String.IsNullOrEmpty(param))
            {
                param = trimmer.Replace(param, " ").Trim();
                devices = devices.Where(p => p.Tittle.Contains(param) || p.Provider.Organization.Contains(param));
            }
            devices = deviceState switch
            {
                DeviceState.TittleDesc => devices.OrderByDescending(s => s.Tittle),
                DeviceState.TypeDeviceDesc => devices.OrderByDescending(s => s.TypeDevice.Tittle),
                DeviceState.TypeDeviceAsc => devices.OrderBy(s => s.TypeDevice.Tittle),
                DeviceState.ProviderAsc => devices.OrderBy(s => s.Provider.Surname),
                DeviceState.ProviderDesc => devices.OrderByDescending(s => s.Provider.Surname),
                DeviceState.DateBuyAsc => devices.OrderBy(s => s.DateBuy),
                DeviceState.DateBuyDesc => devices.OrderByDescending(s => s.DateBuy),
                DeviceState.PriceAsc => devices.OrderBy(s => s.Price),
                DeviceState.PriceDesc => devices.OrderByDescending(s => s.Price),
                _ => devices.OrderBy(s => s.Tittle),
            };
            DeviceViewModel viewModel = new()
            {
                DeviceSort = new DeviceSort(deviceState),
                DeviceFilter = new DeviceFilter(db.TypeDevices.ToList(), typedevice, db.Providers.ToList(), provider, param),
                Devices = devices.ToList(),
            };
            ViewBag.param = param;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ShowChart(int? provider)
        {
            IQueryable<Device> devices = db.Devices.Include(p => p.TypeDevice).Include(p => p.Provider).Where(m => m.Status);
            if (provider != null && provider != 0)
                devices = devices.Where(p => p.ProviderId == provider);
            List<Provider> providers = db.Providers.ToList();
            providers.Insert(0, new Provider { Surname = "Все", Id = 0 });
            SelectList Providers = new(providers, "Id", "Surname", provider);
            ViewBag.Providers = Providers;
            return View(devices);
        }

        [HttpGet]
        public ActionResult CreateDevice()
        {
            SelectList typedevices = new(db.TypeDevices, "Id", "Tittle");
            ViewBag.typedevices = typedevices;
            SelectList providers = new(db.Providers, "Id", "Surname");
            ViewBag.providers = providers;
            return View();
        }

        [HttpPost]
        public ActionResult CreateDevice(Device device)
        {
            SelectList typedevices = new(db.TypeDevices, "Id", "Tittle", device.TypeDeviceId);
            ViewBag.typedevices = typedevices;
            SelectList providers = new(db.Providers, "Id", "Surname", device.ProviderId);
            ViewBag.providers = providers;
            if (ModelState.IsValid)
            {
                if (device.DateBuy.Year < 2020 || device.DateBuy.Date > DateTime.Today)
                {
                    ModelState.AddModelError("DateBuy", $"Укажите корректно дату. Min = 01.01.21 и Max = {DateTime.Today:d}");
                    return View(device);
                }
                device.Tittle = trimmer.Replace(device.Tittle, " ").Trim();
                device.Status = true;
                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("ListDevice");
            }
            return View(device);
        }

        [HttpGet]
        public ActionResult EditDevice(int? deviceid)
        {
            Device device = db.Devices.Find(deviceid);
            if (device == null)
                return HttpNotFound();
            SelectList typedevices = new(db.TypeDevices, "Id", "Tittle", device.TypeDeviceId);
            ViewBag.typedevices = typedevices;
            SelectList providers = new(db.Providers, "Id", "Surname", device.ProviderId);
            ViewBag.providers = providers;
            return View(device);
        }

        [HttpPost]
        public ActionResult EditDevice(Device device)
        {
            SelectList typedevices = new(db.TypeDevices, "Id", "Tittle", device.TypeDeviceId);
            ViewBag.typedevices = typedevices;
            SelectList providers = new(db.Providers, "Id", "Surname", device.ProviderId);
            ViewBag.providers = providers;
            if (ModelState.IsValid)
            {
                device.Tittle = trimmer.Replace(device.Tittle, " ").Trim();
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListDevice");
            }
            return View(device);
        }

        [HttpGet]
        public ActionResult DeleteDevice(int? deviceid)
        {
            Device device = db.Devices.Find(deviceid);
            if (device == null)
                return HttpNotFound();
            device.Status = false;
            db.SaveChanges();
            return RedirectToAction("ListDevice");
        }
    }
}
