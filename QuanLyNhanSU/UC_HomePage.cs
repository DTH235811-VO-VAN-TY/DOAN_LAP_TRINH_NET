using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Chart library

namespace QuanLyNhanSU
{
    public partial class UC_HomePage : UserControl
    {
        // Connection string (Keep your original one)
        string connString = @"Data Source=REDMI-11\SQLEXPRESS01;Initial Catalog=QuanLyNhanSu_DB;Integrated Security=True";

        public UC_HomePage()
        {
            InitializeComponent();
        }

        private void UC_HomePage_Load(object sender, EventArgs e)
        {
            // 1. Load Summary Data (4 colored cards)
            LoadSummaryCards();

            // 2. Draw Professional Charts
            DrawChartDepartment(); // Pie Chart (Department) - Existing
            DrawChartGender();     // Doughnut Chart (Gender) - New
            DrawChartAge();        // Column Chart (Age) - New
            DrawChartSalary();     // Spline Area Chart (Salary) - New
        }

        // --- PART 1: LOAD SUMMARY CARDS (4 TOP CARDS) ---
        private void LoadSummaryCards()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // 1. Total Employees
                    string sqlTongNV = "SELECT COUNT(*) FROM TB_NHANVIEN";
                    SqlCommand cmd1 = new SqlCommand(sqlTongNV, conn);
                    int tongNV = (int)cmd1.ExecuteScalar();
                    lblTongNV.Text = tongNV.ToString("N0"); // Format 1,000

                    // 2. Total Departments
                    string sqlTongPB = "SELECT COUNT(*) FROM TB_PHONGBAN";
                    SqlCommand cmd2 = new SqlCommand(sqlTongPB, conn);
                    int tongPB = (int)cmd2.ExecuteScalar();
                    lblTongPB.Text = tongPB.ToString("N0");

                    // 3. Highest Salary Employee (Based on Actual Salary from tb_BANGLUONG if available, else Contract)
                    // Trying to get from tb_BANGLUONG for accuracy
                    string sqlMaxLuong = @"SELECT TOP 1 nv.HOTEN 
                                           FROM tb_BANGLUONG bl 
                                           JOIN TB_NHANVIEN nv ON bl.MANV = nv.MANV 
                                           ORDER BY bl.THUCLINH DESC";

                    // Fallback to Contract if no Salary computed yet
                    string sqlMaxLuongContract = @"SELECT TOP 1 nv.HOTEN 
                                                   FROM TB_HOPDONG hd 
                                                   JOIN TB_NHANVIEN nv ON hd.MANV = nv.MANV 
                                                   ORDER BY hd.HESOLUONG DESC";

                    SqlCommand cmd4 = new SqlCommand(sqlMaxLuong, conn);
                    object tenMaxLuong = cmd4.ExecuteScalar();

                    if (tenMaxLuong == null) // If no payroll data, use contract data
                    {
                        cmd4.CommandText = sqlMaxLuongContract;
                        tenMaxLuong = cmd4.ExecuteScalar();
                    }
                    lblNVLuongCaoNhat.Text = tenMaxLuong != null ? tenMaxLuong.ToString() : "N/A";


                    // 4. Total Salary (Total 'Actual Received' of the latest month)
                    string sqlTongLuong = @"SELECT SUM(THUCLINH) FROM tb_BANGLUONG 
                                            WHERE MAKYCONG = (SELECT MAX(MAKYCONG) FROM tb_BANGLUONG)";

                    SqlCommand cmd3 = new SqlCommand(sqlTongLuong, conn);
                    object resultLuong = cmd3.ExecuteScalar();

                    if (resultLuong != DBNull.Value && resultLuong != null)
                    {
                        double tongTien = Convert.ToDouble(resultLuong);
                        lblTongLuong.Text = string.Format("{0:N0} VNĐ", tongTien);
                    }
                    else
                    {
                        lblTongLuong.Text = "0 VNĐ";
                    }
                }
                catch (Exception ex)
                {
                    // Fail silently or log for summary cards to allow charts to load
                    lblTongNV.Text = "Error";
                }
            }
        }

        // --- PART 2: DRAW CHARTS ---

        // Chart 1: Personnel by Department (Pie Chart) - Existing logic improved
        private void DrawChartDepartment()
        {
            StyleChart(chartNhanSu, "PERSONNEL BY DEPARTMENT");

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT pb.TENPB, COUNT(nv.MANV) as SoLuong
                                   FROM TB_PHONGBAN pb
                                   LEFT JOIN TB_NHANVIEN nv ON pb.IDPB = nv.IDPB
                                   GROUP BY pb.TENPB
                                   HAVING COUNT(nv.MANV) > 0"; // Only show depts with people

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    chartNhanSu.Series.Clear();
                    Series s = new Series("Series1");
                    s.ChartType = SeriesChartType.Pie;
                    chartNhanSu.Series.Add(s);

                    foreach (DataRow r in dt.Rows)
                    {
                        s.Points.AddXY(r["TENPB"], r["SoLuong"]);
                    }
                    s.IsValueShownAsLabel = true;
                    chartNhanSu.Palette = ChartColorPalette.SeaGreen;
                }
                catch { }
            }
        }

        // Chart 2: Gender Ratio (Doughnut Chart)
        private void DrawChartGender()
        {
            StyleChart(chartGioiTinh, "GENDER RATIO");

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT GIOITINH, COUNT(MANV) FROM TB_NHANVIEN GROUP BY GIOITINH";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    chartGioiTinh.Series.Clear();
                    Series s = new Series("Gender");
                    s.ChartType = SeriesChartType.Doughnut;
                    chartGioiTinh.Series.Add(s);

                    foreach (DataRow r in dt.Rows)
                    {
                        s.Points.AddXY(r[0], r[1]);
                    }
                    s.IsValueShownAsLabel = true;
                    s.Label = "#PERCENT";
                    s.LegendText = "#VALX";
                }
                catch { }
            }
        }

        // Chart 3: Age Structure (Column Chart)
        private void DrawChartAge()
        {
            StyleChart(chartDoTuoi, "AGE STRUCTURE");

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Logic: Calculate Age using DATEDIFF
                    string sql = @"
                        SELECT 
                            CASE 
                                WHEN DATEDIFF(YEAR, NGAYSINH, GETDATE()) < 30 THEN 'Under 30'
                                WHEN DATEDIFF(YEAR, NGAYSINH, GETDATE()) BETWEEN 30 AND 40 THEN '30 - 40'
                                ELSE 'Over 40'
                            END AS AgeGroup,
                            COUNT(MANV) as Count
                        FROM TB_NHANVIEN
                        GROUP BY 
                            CASE 
                                WHEN DATEDIFF(YEAR, NGAYSINH, GETDATE()) < 30 THEN 'Under 30'
                                WHEN DATEDIFF(YEAR, NGAYSINH, GETDATE()) BETWEEN 30 AND 40 THEN '30 - 40'
                                ELSE 'Over 40'
                            END";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    chartDoTuoi.Series.Clear();
                    Series s = new Series("Age");
                    s.ChartType = SeriesChartType.Column;
                    chartDoTuoi.Series.Add(s);

                    foreach (DataRow r in dt.Rows)
                    {
                        s.Points.AddXY(r["AgeGroup"], r["Count"]);
                    }
                    chartDoTuoi.Palette = ChartColorPalette.Pastel;
                }
                catch { }
            }
        }

        // Chart 4: Salary Fluctuation (Spline Area Chart)
        private void DrawChartSalary()
        {
            StyleChart(chartLuong, "SALARY TREND (LAST 6 MONTHS)");

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Get total salary for last 6 months
                    string sql = @"SELECT TOP 6 MAKYCONG, SUM(THUCLINH) as Total 
                                   FROM tb_BANGLUONG 
                                   GROUP BY MAKYCONG 
                                   ORDER BY MAKYCONG ASC";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    chartLuong.Series.Clear();
                    Series s = new Series("Salary");
                    s.ChartType = SeriesChartType.SplineArea;

                    // Styling: Semi-transparent blue fill
                    s.Color = Color.FromArgb(128, 65, 105, 225);
                    s.BorderColor = Color.Blue;
                    s.BorderWidth = 2;

                    chartLuong.Series.Add(s);

                    foreach (DataRow r in dt.Rows)
                    {
                        string month = r["MAKYCONG"].ToString();
                        // Format YYYYMM -> MM/YYYY
                        if (month.Length == 6) month = month.Substring(4, 2) + "/" + month.Substring(0, 4);

                        double amount = Convert.ToDouble(r["Total"]) / 1000000; // Convert to Millions
                        s.Points.AddXY(month, amount);
                    }

                    chartLuong.ChartAreas[0].AxisY.Title = "Unit: Million VND";
                }
                catch { }
            }
        }

        // --- HELPER: COMMON CHART STYLING (CLEAN UI) ---
        private void StyleChart(Chart c, string titleText)
        {
            c.Titles.Clear();
            c.Legends.Clear();
            c.ChartAreas.Clear();

            // 1. Title
            Title t = c.Titles.Add(titleText);
            t.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            t.ForeColor = Color.DimGray;
            t.Alignment = ContentAlignment.TopLeft;

            // 2. Chart Area (Remove Grids)
            ChartArea ca = new ChartArea();
            ca.BackColor = Color.White;

            // Disable vertical grid, make horizontal grid dashed
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisY.MajorGrid.LineColor = Color.LightGray;
            ca.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            // Remove axis lines
            ca.AxisX.LineColor = Color.Transparent;
            ca.AxisY.LineColor = Color.Transparent;

            // Font styling
            ca.AxisX.LabelStyle.Font = new Font("Segoe UI", 8);
            ca.AxisY.LabelStyle.Font = new Font("Segoe UI", 8);
            ca.AxisX.LabelStyle.ForeColor = Color.Gray;
            ca.AxisY.LabelStyle.ForeColor = Color.Gray;

            c.ChartAreas.Add(ca);

            // 3. Legend
            Legend l = new Legend();
            l.Docking = Docking.Top;
            l.Alignment = StringAlignment.Center;
            l.Font = new Font("Segoe UI", 8);
            c.Legends.Add(l);
        }
    }
}