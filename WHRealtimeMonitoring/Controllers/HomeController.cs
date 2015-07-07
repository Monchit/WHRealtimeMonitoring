using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using WHRealtimeMonitoring.Models;

namespace WHRealtimeMonitoring.Controllers
{
    public class HomeController : Controller
    {
        loginicsEntities dbLG = new loginicsEntities();

        [AllowAnonymous]
        public ActionResult Login()
        {
            string username = Request.Form["Username"] != null ? Request.Form["Username"].ToString() : "";
            string password = Request.Form["Password"] != null ? Request.Form["Password"].ToString() : "";
            if (username != "" || password != "")
            {
                if (ModelState.IsValid)
                {
                    var chkuser = (from u in dbLG.tm_user
                                   where u.user_id == username && u.pw == password
                                   select u).FirstOrDefault();
                    if (chkuser != null)
                    {
                        Session["RT_Auth"] = chkuser.user_id;
                        Session["RT_Name"] = chkuser.first_name + " " + chkuser.last_name;
                        Session["RT_UType"] = chkuser.admin_sign;
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("AddDiagramTimeChart", "Home");
        }

        public ActionResult Logout()
        {
            Session.Remove("RT_Auth");
            Session.Remove("RT_Name");
            Session.Remove("RT_UType");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Main";
            ViewBag.Menu = 1;
            return View();
        }

        public ActionResult AddDiagramTimeChart()
        {
            ViewBag.Title = "Add Diagram Time Chart";
            if (Session["RT_UType"] != null && (Session["RT_UType"].ToString() == "4" || Session["RT_UType"].ToString() == "5"))
            {
                ViewBag.Menu = 2;
                return View();
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            ViewBag.Menu = 9;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.Menu = 9;
            return View();
        }

        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        public ActionResult GetTableData(int type)
        {
            var query = from a in dbLG.tt_realtime_monitoring
                        where a.type == type
                        select a;

            int[] pick = new int[4];
            int[] repack = new int[4];
            int[] shipping = new int[4];
            //int[] loading = new int[4];
            foreach (var item in query)
            {
                if (item.process_no == 1) //PICK
                {
                    pick[0] += 1;
                    if (item.status == 1) //COMP
                    {
                        pick[1] += 1;
                    }
                    else if (item.status == 2) //ONPROCESS
                    {
                        pick[2] += 1;
                    }
                    else if (item.status == 3) //DELAY
                    {
                        pick[3] += 1;
                    }
                }
                else if (item.process_no == 2) //REPACK
                {
                    repack[0] += 1;
                    if (item.status == 1) //COMP
                    {
                        repack[1] += 1;
                    }
                    else if (item.status == 2) //ONPROCESS
                    {
                        repack[2] += 1;
                    }
                    else if (item.status == 3) //DELAY
                    {
                        repack[3] += 1;
                    }
                }
                else if (item.process_no == 3) //SHIPPING
                {
                    shipping[0] += 1;
                    if (item.status == 1) //COMP
                    {
                        shipping[1] += 1;
                    }
                    else if (item.status == 2) //ONPROCESS
                    {
                        shipping[2] += 1;
                    }
                    else if (item.status == 3) //DELAY
                    {
                        shipping[3] += 1;
                    }
                }
                //else if (item.process_no == 4) //LOADING
                //{
                //    loading[0] += 1;
                //    if (item.status == 1) //COMP
                //    {
                //        loading[1] += 1;
                //    }
                //    else if (item.status == 2) //ONPROCESS
                //    {
                //        loading[2] += 1;
                //    }
                //    else if (item.status == 3) //DELAY
                //    {
                //        loading[3] += 1;
                //    }
                //}
            }
            var name = "";
            if (type == 1) name = "Milk Run";
            else if (type == 2) name = "Non Milk Run";
            else if (type == 3) name = "Export OEM";
            else if (type == 4) name = "NOK";

            var tbData = "";

            if (type <= 2)
            {
                tbData = @"<table class='table table-condensed table-hover'>
                            <caption><u><b>" + name + @"</b></u></caption>
                            <thead><tr><th>Process</th><th>Total</th><th>Complete</th><th>On Process</th><th>Delay</th></tr>
                            </thead>
                            <tbody>
                            <tr><td>Picking</td><td>" + pick[0] + "</td><td>" + pick[1] + "</td><td>" + pick[2] + "</td><td><a href='' data1='1' data2='" + type + "' class='delayDom'>" + pick[3] + "</a></td></tr>" +
                            "<tr><td>Repack</td><td>" + repack[0] + "</td><td>" + repack[1] + "</td><td>" + repack[2] + "</td><td><a href='' data1='2' data2='" + type + "' class='delayDom'>" + repack[3] + "</a></td></tr>" +
                            "<tr><td>Shipping</td><td>" + shipping[0] + "</td><td>" + shipping[1] + "</td><td>" + shipping[2] + "</td><td><a href='' data1='3' data2='" + type + "' class='delayDom'>" + shipping[3] + "</a></td></tr>" +
                            "</tbody></table>";
            }
            else if (type <= 4)
            {
                tbData = @"<table class='table table-condensed table-hover'>
                            <caption><u><b>" + name + @"</b></u></caption>
                            <thead><tr><th>Process</th><th>Total</th><th>Complete</th><th>On Process</th><th>Delay</th></tr>
                            </thead>
                            <tbody>
                            <tr><td>Picking</td><td>" + pick[0] + "</td><td>" + pick[1] + "</td><td>" + pick[2] + "</td><td><a href='' data1='1' data2='" + type + "' class='delayExp'>" + pick[3] + "</a></td></tr>" +
                            "<tr><td>Repack</td><td>" + repack[0] + "</td><td>" + repack[1] + "</td><td>" + repack[2] + "</td><td><a href='' data1='2' data2='" + type + "' class='delayExp'>" + repack[3] + "</a></td></tr>" +
                            "<tr><td>Shipping</td><td>" + shipping[0] + "</td><td>" + shipping[1] + "</td><td>" + shipping[2] + "</td><td><a href='' data1='3' data2='" + type + "' class='delayExp'>" + shipping[3] + "</a></td></tr>" +
                            "</tbody></table>";
            }
            return Json(tbData, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        public ActionResult GetGraph()
        {
            Dictionary<string, int[]> retVal = new Dictionary<string, int[]>();
            int[] comp = new int[3];
            int[] onprocess = new int[3];
            int[] delay = new int[3];

            foreach (var item in dbLG.tt_realtime_monitoring)
            {
                if (item.status == 1) //COMPLETE
                {
                    comp[item.process_no - 1] += 1;
                }
                else if (item.status == 2) //ONPROCESS
                {
                    onprocess[item.process_no - 1] += 1;
                }
                else if (item.status == 3) //DELAY
                {
                    delay[item.process_no - 1] += 1;
                }
            }

            retVal.Add("Delay", delay);
            retVal.Add("On Process", onprocess);
            retVal.Add("Complete", comp);

            return Json(retVal.ToArray(), JsonRequestBehavior.AllowGet);
        }

        //[OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        //public ActionResult GetDelayData(int process, int type)
        //{
        //    var query = from a in dbLG.tt_realtime_monitoring
        //                join b in dbLG.tt_picking_order_nics on a.tag_no equals b.tag_no
        //                //join c in dbLG.w_zone on a.tag_no 
        //                where a.status == 3 && a.process_no == process && a.type == type
        //                select new { a, b};

        //    var tbdelay = "<table class='table table-condensed table-hover'><thead>"
        //        + "<tr><th>Due Date</th><th>prepare date</th><th>CUST TYPE</th><th>TAGNO</th></tr>"
        //        + "</thead><tbody>";
        //    foreach (var item in query)
        //    {
        //        tbdelay += "<tr><td>" + item.b.co_due_date + "</td><td>" 
        //            + item.b.location_no + "</td><td>" 
        //            + item.b.customer_code + "</td><td>" 
        //            + item.a.tag_no + "</td></tr>";
        //    } 
        //    tbdelay += "</tbody></table>";

        //    return Json(tbdelay, JsonRequestBehavior.AllowGet);
        //}

        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        public ActionResult GetGridDomesticData(string sidx, string sord, int page, int rows, string search, int process, int type)
        {
            var dbSel = from i in ((from a in dbLG.tt_realtime_monitoring
                        join b in dbLG.tt_picking_order_nics on a.tag_no equals b.tag_no
                        join c in dbLG.w_zone on b.customer_code equals c.cust_num
                        where a.status == 3 && a.process_no == process && a.type == type
                        select new {a ,b, c}).ToList())
                        group new { i.a, i.b, i.c } by new
                        {
                            i.a.tag_no,
                            i.b.co_due_date,
                            i.b.finished_goods_code,
                            i.b.customer_code,
                            i.c.cust_name,
                            i.b.co_due_time,
                            i.c.zone,
                            i.b.complete_sign,
                            i.a.finished_time,
                            i.a.process_no,
                            i.b.delivery_place,
                            i.a.prepare_n_date
                            //i.b.box_qty,
                            //i.b.picking_order_qty
                        } into g
                        select new
                        {
                            duedate = g.Key.co_due_date,
                            tagno = g.Key.tag_no.Trim(),
                            item = g.Key.finished_goods_code.Trim(),
                            qty = g.Sum(s => s.b.picking_order_qty),
                            custno = g.Key.customer_code.Trim(),
                            custname = g.Key.cust_name.Trim(),
                            shipto = g.Key.delivery_place.Trim(),
                            cycle = g.Key.co_due_time,
                            zone = g.Key.zone.Trim(),
                            //status = g.Key.complete_sign.Trim() == "" ? "wait income" : ((int)g.Key.picking_order_qty % (int)g.Key.box_qty > 0 ? "wait repack" : "wait ship"),
                            status = g.Key.complete_sign.Trim() == "" ? "Not yet" : (g.Key.complete_sign.Trim() == "1" ? "Picked" : "Shipped"),
                            shipvia = "Ship Via",
                            prepare = g.Key.prepare_n_date != null ? "N-" + g.Key.prepare_n_date : "",
                            finish = g.Key.finished_time
                        };

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = dbSel.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);

            // This is possible because I'm using the LINQ Dynamic Query Library
            var delay = dbSel.AsQueryable()
                    .OrderBy(sidx + " " + sord)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize).AsEnumerable();

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = delay.ToList().Select(d => new
                {
                    i = d.tagno,
                    cell = new object[] {
                        ChangeDateDisplay(d.duedate.ToString()),
                        d.tagno,
                        d.item,
                        d.qty,
                        d.custno,
                        d.custname,
                        d.shipto,
                        d.cycle,
                        d.zone,
                        d.status,
                        d.shipvia,
                        d.prepare,
                        ChangeDateDisplay(d.finish)
                    }
                }).ToArray()
            };
            return Json(jsonData);
        }

        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        public ActionResult GetGridOEMData(string sidx, string sord, int page, int rows, string search, int process, int type)
        {
            var dbSel1 = from a in dbLG.tt_realtime_monitoring.GroupBy(g => new { g.tag_no, g.status, g.process_no, g.type, g.finished_time })
                             .Where(w => w.Key.status == 3 && w.Key.process_no == process && w.Key.type == type)
                         join b in dbLG.tt_packing_order_nics
                         .GroupBy(g => new { g.picking_order_no, g.customer_code, g.data_tran_date, g.complete_sign, g.finished_goods_code })
                         .Select(s => new { s.Key, Qty = s.Sum(m => m.picking_order_qty) }) on a.Key.tag_no equals b.Key.picking_order_no into ab
                         from b in ab.DefaultIfEmpty()
                         join c in dbLG.w_zone.GroupBy(g => new { g.cust_num, g.cust_name }) on b.Key.customer_code equals c.Key.cust_num into bc
                         from c in bc.DefaultIfEmpty()
                         join d in dbLG.tm_item_summary.GroupBy(g => g.customer_type) on b.Key.customer_code equals d.Key into bd
                         from d in bd.DefaultIfEmpty()
                         select new
                         {
                             process = a.Key.process_no == 1 ? "PICK" : (a.Key.process_no == 2 ? "REPACK" : "SHIP"),
                             trandate = b.Key.data_tran_date,
                             type = a.Key.type == 4 ? "NOK" : "EOEM",
                             tagno = a.Key.tag_no.Trim(),
                             item = b.Key.finished_goods_code.Trim(),
                             qty = b.Qty,
                             custno = b.Key.customer_code,//g.Key.customer_code.Trim(),
                             custname = c.Key.cust_name,
                             status = b.Key.complete_sign.Trim() == "" ? "Not yet" : (b.Key.complete_sign.Trim() == "1" ? "Picked" : "Shipped"),
                             finish = a.Key.finished_time
                         };

            //var dbSel = from i in
            //                ((from a in dbLG.tt_realtime_monitoring
            //                  join b in dbLG.tt_packing_order_nics on a.tag_no equals b.picking_order_no
            //                  join c in dbLG.w_zone on b.customer_code equals c.cust_num
            //                  join d in dbLG.tm_item_summary on b.finished_goods_code equals d.finished_goods_code
            //                  where a.status == 3 && a.process_no == process && a.type == type //&& d.customer_type == "AFF" 
            //                  //group new { b, c, d } by new { b.picking_order_no, b.customer_code, c.cust_name, d.customer_type } into g
            //                  select new { a, b, c, d }).ToList())
            //            group new { i.a, i.b, i.c, i.d } by new
            //            {
            //                i.b.picking_order_no,
            //                i.a.tag_no,
            //                i.b.data_tran_date,
            //                i.b.finished_goods_code,
            //                i.b.customer_code,
            //                i.c.cust_name,
            //                //i.b.co_due_time,
            //                //i.c.zone,
            //                i.b.complete_sign,
            //                i.a.finished_time,
            //                i.a.process_no,
            //                i.d.customer_type
            //                //i.b.delivery_place,
            //                //i.a.prepare_n_date
            //                //i.b.box_qty,
            //                //i.b.picking_order_qty
            //            } into g
            //            select new
            //            {
            //                process = g.Key.process_no == 1 ? "PICK" : (g.Key.process_no == 2 ? "REPACK" : "SHIP"),
            //                trandate = g.Key.data_tran_date,
            //                type = g.Key.customer_type == "AFF" ? "NOK" : "EOEM",
            //                tagno = g.Key.tag_no.Trim(),
            //                item = g.Key.finished_goods_code.Trim(),
            //                qty = g.Sum(s => s.b.picking_order_qty),
            //                custno = g.Key.customer_code.Trim(),
            //                custname = g.Key.cust_name.Trim(),
            //                //shipto = g.Key.delivery_place.Trim(),
            //                //cycle = g.Key.co_due_time,
            //                //zone = g.Key.zone.Trim(),
            //                //status = g.Key.complete_sign.Trim() == "" ? "wait income" : ((int)g.Key.picking_order_qty % (int)g.Key.box_qty > 0 ? "wait repack" : "wait ship"),
            //                status = g.Key.complete_sign.Trim() == "" ? "Not yet" : (g.Key.complete_sign.Trim() == "1" ? "Picked" : "Shipped"),
            //                //shipvia = "Ship Via",
            //                //prepare = g.Key.prepare_n_date != null ? "N-" + g.Key.prepare_n_date : "",
            //                finish = g.Key.finished_time
            //            };

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = dbSel1.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);

            // This is possible because I'm using the LINQ Dynamic Query Library
            var delay = dbSel1.AsQueryable()
                    .OrderBy(sidx + " " + sord)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize).AsEnumerable();

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = delay.ToList().Select(d => new
                {
                    i = d.tagno,
                    cell = new object[] {
                        d.process,
                        ChangeDateDisplay(d.trandate.ToString()),
                        d.type,
                        d.tagno,
                        d.item,
                        d.qty,
                        d.custno,
                        d.custname,
                        //d.shipto,d.cycle,d.zone,
                        d.status,
                        //d.shipvia,d.prepare,
                        ChangeDateDisplay(d.finish)
                    }
                }).ToArray()
            };

            return Json(jsonData);
        }

        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        public ActionResult GridExportToExcel(string sidx, string sord, int process, int type)
        {
            var gridModel = new GridExportModel();
            var grid = gridModel.DelayGrid;

            var dbSel = (from i in
                             ((from a in dbLG.tt_realtime_monitoring
                               join b in dbLG.tt_picking_order_nics on a.tag_no equals b.tag_no
                               join c in dbLG.w_zone on b.customer_code equals c.cust_num
                               where a.status == 3 && a.process_no == process && a.type == type
                               select new { a, b, c }).ToList())
                         group new { i.a, i.b, i.c } by new
                         {
                             i.a.tag_no,
                             i.b.co_due_date,
                             i.b.finished_goods_code,
                             i.b.customer_code,
                             i.c.cust_name,
                             i.b.co_due_time,
                             i.c.zone,
                             i.b.complete_sign,
                             i.a.finished_time,
                             i.a.process_no,
                             i.b.delivery_place,
                             i.a.prepare_n_date
                             //i.b.box_qty,
                             //i.b.picking_order_qty
                         } into g
                         select new
                         {
                             duedate = g.Key.co_due_date,
                             tagno = g.Key.tag_no.Trim(),
                             item = g.Key.finished_goods_code.Trim(),
                             qty = g.Sum(s => s.b.picking_order_qty),
                             custno = g.Key.customer_code.Trim(),
                             custname = g.Key.cust_name.Trim(),
                             shipto = g.Key.delivery_place.Trim(),
                             cycle = g.Key.co_due_time,
                             zone = g.Key.zone.Trim(),
                             //status = g.Key.complete_sign.Trim() == "" ? "wait income" : ((int)g.Key.picking_order_qty % (int)g.Key.box_qty > 0 ? "wait repack" : "wait ship"),
                             status = g.Key.complete_sign.Trim() == "" ? "Not yet" : (g.Key.complete_sign.Trim() == "1" ? "Picked" : "Shipped"),
                             shipvia = "Ship Via",
                             prepare = g.Key.prepare_n_date != null ? "N-" + g.Key.prepare_n_date : "",
                             finish = g.Key.finished_time
                         }).AsQueryable().OrderBy(sidx + " " + sord);

            var output = from d in dbSel.ToList()
                         select new
                         {
                             duedate = ChangeDateDisplay(d.duedate.ToString()),
                             d.tagno,
                             d.item,
                             d.qty,
                             d.custno,
                             d.custname,
                             d.shipto,
                             d.cycle,
                             d.zone,
                             d.status,
                             d.shipvia,
                             d.prepare,
                             finish = ChangeDateDisplay(d.finish)
                         };

            grid.ExportToExcel(output, "DomesticDelay.xls");

            return View();
        }

        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        public ActionResult GridExportToExcel1(string sidx1, string sord1, int process1, int type1)
        {
            var gridModel = new OEMModel();
            var grid = gridModel.OEMGrid;

            var dbSel = (from i in
                             ((from a in dbLG.tt_realtime_monitoring
                               join b in dbLG.tt_packing_order_nics on a.tag_no equals b.picking_order_no
                               join c in dbLG.w_zone on b.customer_code equals c.cust_num
                               join d in dbLG.tm_item_summary on b.finished_goods_code equals d.finished_goods_code
                               where a.status == 3 && d.customer_type == "AFF" //&& a.process_no == process && a.type == type
                               select new { a, b, c, d }).ToList())
                         group new { i.a, i.b, i.c, i.d } by new
                         {
                             i.a.tag_no,
                             i.b.data_tran_date,
                             i.b.finished_goods_code,
                             i.b.customer_code,
                             i.c.cust_name,
                             //i.b.co_due_time,
                             //i.c.zone,
                             i.b.complete_sign,
                             i.a.finished_time,
                             i.a.process_no,
                             i.d.customer_type
                             //i.b.delivery_place,
                             //i.a.prepare_n_date
                             //i.b.box_qty,
                             //i.b.picking_order_qty
                         } into g
                         select new
                         {
                             process = g.Key.process_no == 1 ? "PICK" : (g.Key.process_no == 2 ? "REPACK" : "SHIP"),
                             trandate = g.Key.data_tran_date,
                             type = g.Key.customer_type == "AFF" ? "NOK" : "EOEM",
                             tagno = g.Key.tag_no.Trim(),
                             item = g.Key.finished_goods_code.Trim(),
                             qty = g.Sum(s => s.b.picking_order_qty),
                             custno = g.Key.customer_code.Trim(),
                             custname = g.Key.cust_name.Trim(),
                             //shipto = g.Key.delivery_place.Trim(),
                             //cycle = g.Key.co_due_time,
                             //zone = g.Key.zone.Trim(),
                             //status = g.Key.complete_sign.Trim() == "" ? "wait income" : ((int)g.Key.picking_order_qty % (int)g.Key.box_qty > 0 ? "wait repack" : "wait ship"),
                             status = g.Key.complete_sign.Trim() == "" ? "Not yet" : (g.Key.complete_sign.Trim() == "1" ? "Picked" : "Shipped"),
                             //shipvia = "Ship Via",
                             //prepare = g.Key.prepare_n_date != null ? "N-" + g.Key.prepare_n_date : "",
                             finish = g.Key.finished_time
                         }).AsQueryable().OrderBy(sidx1 + " " + sord1);

            var output = from d in dbSel.ToList()
                         select new
                         {
                             d.process,
                             trandate = ChangeDateDisplay(d.trandate.ToString()),
                             d.type,
                             d.tagno,
                             d.item,
                             d.qty,
                             d.custno,
                             d.custname,
                             //d.shipto,
                             //d.cycle,
                             //d.zone,
                             d.status,
                             //d.shipvia,
                             //d.prepare,
                             finish = ChangeDateDisplay(d.finish)
                         };

            grid.ExportToExcel(output, "ExportDelay.xls");

            return View();
        }

        [HttpPost]
        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        public ActionResult GridExportTimeChart(string sidx, string sord, string xcusttype, string xcustno)
        {
            var gridModel = new TimeChartExportModel();
            var grid = gridModel.TimeChartGrid;

            var dbSel = dbLG.tm_diagram_time_chart.OrderBy(sidx + " " + sord);

            if (!string.IsNullOrEmpty(xcustno))
                dbSel = dbSel.Where(w => w.cust_no.Trim() == xcustno);

            if (!string.IsNullOrEmpty(xcusttype))
                dbSel = dbSel.Where(w => w.cust_type.Trim() == xcusttype);


            var output = from c in dbSel.ToList()
                         select new
                         {
                            cust_no = "'" + c.cust_no,
                            process_no = GetProcessName(c.process_no),
                            start_time = ChangeTimeDisplay(c.start_time),
                            finished_time = ChangeTimeDisplay(c.finished_time),
                            c.cycle_no,
                            prepare_n_date = "N-" + c.prepare_n_date,
                            c.shipto,
                            update_date = ChangeDateDisplay(c.update_date),
                            update_time = ChangeTimeDisplay(c.update_time),
                            username_id = GetFullName(c.username_id),
                            cust_type = GetCustType(c.cust_type)
                         };

            grid.ExportToExcel(output, "TimeChart.xls");

            return View();
        }

        [OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
        public ActionResult GetGridTimeChart(string sidx, string sord, int page, int rows, string search, string custno, string custtype)
        {
            var dbSel = from d in dbLG.tm_diagram_time_chart
                        select d;
            if (!string.IsNullOrEmpty(custno))
                dbSel = dbSel.Where(w => w.cust_no.Trim() == custno);

            if (!string.IsNullOrEmpty(custtype))
                dbSel = dbSel.Where(w => w.cust_type.Trim() == custtype);

            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = dbSel.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);

            // This is possible because I'm using the LINQ Dynamic Query Library
            var query = dbSel.OrderBy(sidx + " " + sord)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize).AsEnumerable();

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = query.ToList().Select(c => new
                {
                    i = c.cust_no,
                    cell = new object[] {
                        c.cust_no,
                        GetProcessName(c.process_no),
                        ChangeTimeDisplay(c.start_time),
                        ChangeTimeDisplay(c.finished_time),
                        c.cycle_no,
                        "N-" + c.prepare_n_date,
                        c.shipto,
                        ChangeDateDisplay(c.update_date),
                        ChangeTimeDisplay(c.update_time),
                        GetFullName(c.username_id),
                        GetCustType(c.cust_type),
                        "<a href='#' class='btGet' data-custno=" + c.cust_no + " data-process=" + c.process_no + " data-cycle=" + c.cycle_no + " data-shipto=" + c.shipto + "><img src='" + Url.Content("~/" + "Images/pencil.png") + "' /></a>",
                        "<a href='#' class='btDel' data-custno=" + c.cust_no + " data-process=" + c.process_no + " data-cycle=" + c.cycle_no + " data-shipto=" + c.shipto + "><img src='" + Url.Content("~/" + "Images/remove.png") + "' /></a>"
                    }
                }).ToArray()
            };

            return Json(jsonData);
        }

        public ActionResult GetTimeChart(string cust, string process, int cycle, string shipto)
        {
            var dbSel = (from d in dbLG.tm_diagram_time_chart.ToList()
                         where d.cust_no == cust && d.process_no == process && d.cycle_no == cycle && d.shipto == shipto
                         select new { 
                            d.cust_no,
                            d.cust_type,
                            d.process_no,
                            start_time = d.start_time.Substring(0,2) + ":" + d.start_time.Substring(2,2),
                            finished_time = d.finished_time.Substring(0, 2) + ":" + d.finished_time.Substring(2, 2),
                            d.cycle_no,
                            d.prepare_n_date,
                            d.shipto
                         }).FirstOrDefault();

            return Json(dbSel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddTimeChart()
        {
            try
            {
                if (Session["RT_Auth"] != null)
                {
                    var tbTimeChart = new tm_diagram_time_chart();
                    tbTimeChart.cust_no = Request.Form["custno"];
                    tbTimeChart.cust_type = Request.Form["custtype"];
                    tbTimeChart.process_no = Request.Form["process"];
                    tbTimeChart.start_time = Request.Form["start"].Replace(":", "");
                    tbTimeChart.finished_time = Request.Form["finish"].ToString().Replace(":", "");
                    tbTimeChart.cycle_no = Request.Form["cycle"] == null ? 0 : int.Parse(Request.Form["cycle"]);
                    tbTimeChart.prepare_n_date = Request.Form["prepare"] == null ? 0 : int.Parse(Request.Form["prepare"]);
                    tbTimeChart.shipto = Request.Form["shipto"];
                    tbTimeChart.username_id = Session["RT_Auth"].ToString();
                    tbTimeChart.update_date = DateTime.Now.ToString("yyyyMMdd");
                    tbTimeChart.update_time = DateTime.Now.ToString("HHmm");
                    dbLG.tm_diagram_time_chart.Add(tbTimeChart);
                    dbLG.SaveChanges();
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult UpdateTimeChart()
        {
            try
            {
                if (Session["RT_Auth"] != null)
                {
                    var custno = Request.Form["custno"];
                    var process = Request.Form["process"];
                    int cycle = Request.Form["cycle"] == null ? 0 : int.Parse(Request.Form["cycle"]);
                    var shipto = Request.Form["shipto"];
                    var query = (from t in dbLG.tm_diagram_time_chart
                                 where t.cust_no == custno && t.process_no == process && t.cycle_no == cycle && t.shipto == shipto
                                 select t).FirstOrDefault();
                    
                    query.cust_type = Request.Form["custtype"];
                    query.start_time = Request.Form["start"].Replace(":", "");
                    query.finished_time = Request.Form["finish"].Replace(":", "");
                    query.prepare_n_date = Request.Form["prepare"] == null ? 0 : int.Parse(Request.Form["prepare"]);
                    query.username_id = Session["RT_Auth"].ToString();
                    query.update_date = DateTime.Now.ToString("yyyyMMdd");
                    query.update_time = DateTime.Now.ToString("HHmm");
                    dbLG.SaveChanges();

                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult DeleteTimeChart()
        {
            try
            {
                var custno = Request.Form["custno"];
                var process = Request.Form["process"];
                int cycle = Request.Form["cycle"] == null ? 0 : int.Parse(Request.Form["cycle"]);
                var shipto = Request.Form["shipto"];
                var query = dbLG.tm_diagram_time_chart.Find(custno,process,cycle,shipto);
                dbLG.tm_diagram_time_chart.Remove(query);
                dbLG.SaveChanges();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        public string ChangeDateDisplay(string sdate)
        {
            if (!string.IsNullOrEmpty(sdate))
            {
                string newdate = sdate.Substring(0, 4) + "-" + sdate.Substring(4, 2) + "-" + sdate.Substring(6, 2);
                if (sdate.Length == 12)
                {
                    newdate += " " + sdate.Substring(8, 2) + ":" + sdate.Substring(10, 2);
                }
                return newdate;
            }
            return sdate;
        }

        public string ChangeTimeDisplay(string stime)
        {
            if (!string.IsNullOrEmpty(stime))
            {
                string newtime = stime.Substring(0, 2) + ":" + stime.Substring(2, 2);
                return newtime;
            }
            return stime;
        }

        public string GetProcessName(string process_no)
        {
            string process_name = "";
            if (!string.IsNullOrEmpty(process_no))
            {
                switch (process_no)
                {
                    case "1": process_name = "PICKING"; break;
                    case "2": process_name = "REPACK"; break;
                    case "3": process_name = "SHIPPING"; break;
                    case "4": process_name = "LOADING"; break;
                    
                }
            }
            return process_name;
        }

        public string GetCustType(string cust_type)
        {
            string custtype_name = "";
            if (!string.IsNullOrEmpty(cust_type))
            {
                switch (cust_type)
                {
                    case "1": custtype_name = "Milk Run"; break;
                    case "2": custtype_name = "Non Milk Run"; break;
                    case "3": custtype_name = "Export OEM"; break;
                    case "4": custtype_name = "NOK"; break;
                }
            }
            return custtype_name;
        }

        public string GetFullName(string user_code)
        {
            var query = (from a in dbLG.tm_user
                         where a.user_id == user_code
                         select a).FirstOrDefault();
            if (query != null)
            {
                return query.first_name + " " + query.last_name;
            }
            else
            {
                return user_code;
            }
        }
    }
}
