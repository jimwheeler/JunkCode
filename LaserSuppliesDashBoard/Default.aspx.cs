using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;

namespace LaserSuppliesDashBoard
{
    public partial class Default : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                UpdateData();
                BuildSlides();
            }
        }



        private void BuildSlides()
        {
            try
            {
                string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Data/Slides"));
                StringBuilder sb = new StringBuilder();
                List<FileInfo> slideFiles = new List<FileInfo>();

                string sitePath = Request.Path.ToLower();
                sitePath = sitePath.Replace("default.aspx", "");
                if (sitePath.EndsWith("/")) sitePath = sitePath + "Data/Slides/";
                else sitePath = sitePath + "/Data/Slides/";
                
                // STICK ON A DUMMY QUERY PARAMETER IN ORDER TO FORCE THE IMAGE TO BE
                // RELOADED INSTEAD OF BEING PULLED FROM CACHE
                foreach (string fileName in files)
                {
                    FileInfo fi = new FileInfo(fileName);
                    slideFiles.Add(fi);
                    sb.Append(string.Format("<li><img src='{0}{1}?fi={2}' width='540' height='380'/></li>\r\n", sitePath, Path.GetFileName(fileName), DateTime.Now.Ticks));
                }

                Session["SlideFiles"] = slideFiles;
                Literal1.Text = sb.ToString();

                ScriptManager.RegisterStartupScript(Literal1, typeof(Control), "ScrollingStuff", "<script type='text/javascript'>$('#scroller').simplyScroll();</script>", false);

            }
            catch
            {
                Literal1.Text = "<li>Something Wrong With Slide Files</li>";
            }

        }




        private void UpdateData()
        {
            try
            {
                using (DataTable tbl = new DataTable())
                {
                    string[] lines = File.ReadAllLines(HttpContext.Current.Server.MapPath("~/Data/Data.csv"));
                    string[] columns = lines[0].Split(new char[] { ',' });
                    foreach (string colName in columns)
                    {
                        if (colName.ToUpper() == "URGENCY LEVEL") continue;
                        if (colName.ToUpper() == "DATE CREATED") continue;
                        if (colName.ToUpper() == "DATE ANSWERED") continue;
                        if (colName == null || colName.Length <= 0) continue;
                        tbl.Columns.Add(colName, typeof(string));
                    }


                    for (int i = 1; i < lines.Length; i++)  // START AT 1 BECAUSE LINE 0 CONTAINS COLUMNS HEADERS
                    {
                        string[] fields = lines[i].Split(new char[] { ',' });

                        if (fields[0] == "") break;

                        DataRow dr = tbl.NewRow();

                        for (int j = 0; j < tbl.Columns.Count; j++)  // START AT 3 BECAUSE FIRST 3 COLUMNS CONTAIN NON DISPLAY DATA
                        {
                            dr[j] = fields[j + 3];
                        }

                        tbl.Rows.Add(dr);
                    }

                    GridView1.Caption = "";
                    GridView1.DataSource = tbl;
                    GridView1.DataBind();

                }
            }
            catch
            {
                GridView1.Caption = "Data File Is Invalid";
            }

        }


        
        protected void GridView1_DataBound(object sender, EventArgs e)
        {

            string[] lines = File.ReadAllLines(HttpContext.Current.Server.MapPath("~/Data/Data.csv"));

            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(new char[] { ',' });

                if (fields[0] == "") break;

                DateTime endDate;
                DateTime startDate = DateTime.Parse(fields[1]);
                endDate = DateTime.Now;
                
                if (fields[2] != "")
                {
                    endDate = DateTime.Parse(fields[2]);
                }

                TimeSpan ts  = endDate - startDate;
                int days = (int)ts.TotalDays;
                GridView1.Rows[i - 1].Cells[6].Text = days.ToString();
                GridView1.Rows[i - 1].Cells[6].HorizontalAlign = HorizontalAlign.Center;
                
                if (days > 30)
                {
                    GridView1.Rows[i - 1].Cells[6].BackColor = Color.Red;

                }
                else if (days > 20)
                {
                    GridView1.Rows[i - 1].Cells[6].BackColor = Color.Yellow;
                }
                else
                {
                    GridView1.Rows[i - 1].Cells[6].BackColor = Color.LightGreen;
                }
                

                if (fields[0].ToUpper() == "RED")
                {
                    GridView1.Rows[i - 1].Cells[0].BackColor = Color.Red;
                }
                else if (fields[0].ToUpper() == "YELLOW")
                {
                    GridView1.Rows[i - 1].Cells[0].BackColor = Color.Yellow;
                }
                else if (fields[0].ToUpper() == "GREEN")
                {
                    GridView1.Rows[i - 1].Cells[0].BackColor = Color.LightGreen;
                }

            }
            
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            UpdateData();
        }




        protected void Timer2_Tick(object sender, EventArgs e)
        {
            // CHECK TO SEE IF SLIDES NEED TO BE UPDATED
            List<FileInfo> oldSlideFiles = (List<FileInfo>)Session["SlideFiles"];

            if (oldSlideFiles == null)
            {
                BuildSlides();
                UpdatePanel2.Update();
                return;
            }
            
            string[] files = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Data/Slides"));
            List<FileInfo> slideFiles = new List<FileInfo>();

            foreach (string fileName in files)
            {
                FileInfo fi = new FileInfo(fileName);
                slideFiles.Add(fi);
            }


            // DIFFERENT NUMBER OF FILES TO REBUILD
            if (slideFiles.Count != oldSlideFiles.Count)
            {
                BuildSlides();
                UpdatePanel2.Update();
                return;
            }

            // SAME NUMBER OF FILES SO SEE IF ANY FILE HAS CHANGED
            foreach (FileInfo fi in slideFiles)
            {
                bool match_found = false;

                foreach (FileInfo fi2 in oldSlideFiles)
                {
                    if (fi2.FullName == fi.FullName && fi2.LastWriteTime == fi.LastWriteTime && fi2.Length == fi.Length)
                    {
                        match_found = true;
                        break;
                    }
                }

                if (match_found == false)
                {
                    BuildSlides();
                    UpdatePanel2.Update();
                    return;
                }
            }

        }



    }
}