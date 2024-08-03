using ClosedXML.Excel;
using ComputerAccounting.App_Data;
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
    public class ProviderController : Controller
    {
        CompContent db = new();
        static readonly Regex trimmer = new(@"\s\s+");

        public ActionResult ExportProvider()
        {
            List<Provider> providers = db.Providers.Where(m => m.Status).ToList();
            XLWorkbook workbook = new();
            var worksheet = workbook.Worksheets.Add("Поставщики");
            worksheet.Cell("A1").Value = "ФИО";
            worksheet.Cell("B1").Value = "Номер телефона";
            worksheet.Cell("C1").Value = "Организация";
            worksheet.Cell("D1").Value = "Адрес";
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Range($"A1:D{providers.Count + 1}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{providers.Count + 1}").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
            worksheet.Range($"A1:D{providers.Count + 1}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{providers.Count + 1}").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{providers.Count + 1}").Style.Border.RightBorder = XLBorderStyleValues.Thin;
            worksheet.Range($"A1:D{providers.Count + 1}").Style.Border.TopBorder = XLBorderStyleValues.Thin;
            for (int i = 0; i < providers.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = providers[i].Surname + " " + providers[i].Name + " " + providers[i].Patronymic;
                worksheet.Cell(i + 2, 2).Value = providers[i].PhoneNumber;
                worksheet.Cell(i + 2, 3).Value = providers[i].Organization;
                worksheet.Cell(i + 2, 4).Value = providers[i].OrganizationAdress;
            }
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Flush();
            return new FileContentResult(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"Поставщики_{DateTime.UtcNow.ToShortDateString()}.xlsx"
            };
        }

        [HttpGet]
        public ActionResult ListProvider(string param, ProviderState providerState = ProviderState.SurnameAsc)
        {
            IQueryable<Provider> providers = db.Providers.Where(m => m.Status);
            if (!String.IsNullOrEmpty(param))
            {
                param = trimmer.Replace(param, " ").Trim();
                providers = providers.Where(p => p.Name.Contains(param) || p.Surname.Contains(param) || p.Patronymic.Contains(param) || p.Organization.Contains(param) || p.PhoneNumber.Contains(param));
            }
            providers = providerState switch
            {
                ProviderState.SurnameDesc => providers.OrderByDescending(s => s.Surname),
                ProviderState.OrganizationDesc => providers.OrderByDescending(s => s.Organization),
                ProviderState.OrganizationAsc => providers.OrderBy(s => s.Organization),
                _ => providers.OrderBy(s => s.Surname),
            };
            ProviderViewModel viewModel = new()
            {
                SortViewModel = new ProviderSort(providerState),
                FilterViewModel = new ProviderFilter(param),
                Providers = providers.ToList()
            };
            ViewBag.param = param;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult CreateProvider() => View();

        [HttpPost]
        public ActionResult CreateProvider(Provider provider)
        {
            if (ModelState.IsValid)
            {
                provider.Organization = trimmer.Replace(provider.Organization, " ").Trim();
                provider.OrganizationAdress = trimmer.Replace(provider.OrganizationAdress, " ").Trim();
                provider.Status = true;
                db.Providers.Add(provider);
                db.SaveChanges();
                return RedirectToAction("ListProvider");
            }
            return View(provider);
        }

        [HttpGet]
        public ActionResult EditProvider(int? providerid)
        {
            Provider Provider = db.Providers.Find(providerid);
            if (providerid == null)
                return HttpNotFound();
            return View(Provider);
        }

        [HttpPost]
        public ActionResult EditProvider(Provider provider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListProvider");
            }
            return View(provider);
        }

        [HttpGet]
        public ActionResult DeleteProvider(int? providerid)
        {
            Provider provider = db.Providers.Find(providerid);
            if (providerid == null)
                return HttpNotFound();
            provider.Status = false;
            db.SaveChanges();
            return RedirectToAction("ListProvider");
        }
    }
}
