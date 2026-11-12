
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bai54
{
    public partial class Form1 : Form
    {
        // Lớp lưu thông tin nhân viên
        public class NhanVien
        {
            public string MaNV { get; set; }
            public string HoTen { get; set; }
            public string ChucVu { get; set; }
            public DateTime NgayVaoLam { get; set; }
            public string PhongBan { get; set; }
            public string Nhom { get; set; }
        }

        private SplitContainer splitContainer;
        private TreeView tvDepartments;
        private ListView lsvEmployees;
        private ComboBox cboView;
        private ImageList imageList;

        private List<NhanVien> danhSachNV = new List<NhanVien>();

        public Form1()
        {
            InitializeComponent();

            TaoGiaoDien();
            TaoDuLieuMau();
            TaoCayPhongBan();
        }

        // 1. Tạo giao diện
        private void TaoGiaoDien()
        {
            this.Text = "BÀI 5.4 - QUẢN LÝ NHÂN VIÊN";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel phía trên
            Panel pnlTop = new Panel();
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 50;
            pnlTop.BackColor = Color.LightSteelBlue;

            Label lblView = new Label();
            lblView.Text = "Chế độ hiển thị:";
            lblView.Location = new Point(15, 15);
            lblView.AutoSize = true;

            cboView = new ComboBox();
            cboView.Location = new Point(125, 11);
            cboView.Width = 150;
            cboView.DropDownStyle = ComboBoxStyle.DropDownList;

            cboView.Items.AddRange(new object[]
            {
                "Details",
                "SmallIcon",
                "LargeIcon",
                "Tile"
            });

            cboView.SelectedIndex = 0;
            cboView.SelectedIndexChanged += CboView_SelectedIndexChanged;

            pnlTop.Controls.Add(lblView);
            pnlTop.Controls.Add(cboView);

            // SplitContainer
            splitContainer = new SplitContainer();
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.SplitterDistance = 280;

            // TreeView
            tvDepartments = new TreeView();
            tvDepartments.Dock = DockStyle.Fill;
            tvDepartments.Font = new Font("Arial", 10);
            tvDepartments.AfterSelect += TvDepartments_AfterSelect;

            // ListView
            lsvEmployees = new ListView();
            lsvEmployees.Dock = DockStyle.Fill;
            lsvEmployees.View = View.Details;
            lsvEmployees.FullRowSelect = true;
            lsvEmployees.GridLines = true;
            lsvEmployees.MultiSelect = false;

            lsvEmployees.Columns.Add("Mã NV", 100);
            lsvEmployees.Columns.Add("Họ Tên", 200);
            lsvEmployees.Columns.Add("Chức vụ", 150);
            lsvEmployees.Columns.Add("Ngày vào làm", 150);

            // ImageList
            imageList = new ImageList();
            imageList.ImageSize = new Size(32, 32);
            imageList.ColorDepth = ColorDepth.Depth32Bit;

            imageList.Images.Add(SystemIcons.Application);
            imageList.Images.Add(SystemIcons.Information);
            imageList.Images.Add(SystemIcons.Shield);

            tvDepartments.ImageList = imageList;

            lsvEmployees.SmallImageList = imageList;
            lsvEmployees.LargeImageList = imageList;

            lsvEmployees.TileSize = new Size(350, 70);

            // Đưa các control vào SplitContainer
            splitContainer.Panel1.Controls.Add(tvDepartments);
            splitContainer.Panel2.Controls.Add(lsvEmployees);

            // Đưa vào Form
            this.Controls.Add(splitContainer);
            this.Controls.Add(pnlTop);
        }

        // 2. Tạo dữ liệu nhân viên mẫu
        private void TaoDuLieuMau()
        {
            danhSachNV.Add(new NhanVien
            {
                MaNV = "NV001",
                HoTen = "Nguyễn Văn An",
                ChucVu = "Trưởng nhóm",
                NgayVaoLam = new DateTime(2022, 3, 15),
                PhongBan = "KyThuat",
                Nhom = "PhatTrien"
            });

            danhSachNV.Add(new NhanVien
            {
                MaNV = "NV002",
                HoTen = "Trần Văn Bình",
                ChucVu = "Lập trình viên",
                NgayVaoLam = new DateTime(2023, 5, 10),
                PhongBan = "KyThuat",
                Nhom = "PhatTrien"
            });

            danhSachNV.Add(new NhanVien
            {
                MaNV = "NV003",
                HoTen = "Lê Thị Hoa",
                ChucVu = "Tester",
                NgayVaoLam = new DateTime(2023, 8, 20),
                PhongBan = "KyThuat",
                Nhom = "KiemThu"
            });

            danhSachNV.Add(new NhanVien
            {
                MaNV = "NV004",
                HoTen = "Phạm Văn Nam",
                ChucVu = "Trưởng phòng",
                NgayVaoLam = new DateTime(2021, 1, 12),
                PhongBan = "KeToan",
                Nhom = "KeToanTongHop"
            });

            danhSachNV.Add(new NhanVien
            {
                MaNV = "NV005",
                HoTen = "Hoàng Thị Lan",
                ChucVu = "Kế toán viên",
                NgayVaoLam = new DateTime(2024, 2, 5),
                PhongBan = "KeToan",
                Nhom = "KeToanTongHop"
            });

            danhSachNV.Add(new NhanVien
            {
                MaNV = "NV006",
                HoTen = "Đỗ Văn Hùng",
                ChucVu = "Nhân viên",
                NgayVaoLam = new DateTime(2022, 7, 18),
                PhongBan = "NhanSu",
                Nhom = "TuyenDung"
            });
        }

        // 3. Tạo cây phòng ban
        private void TaoCayPhongBan()
        {
            tvDepartments.Nodes.Clear();

            TreeNode root = new TreeNode("CÔNG TY ABC");
            root.Tag = "CongTy";
            root.ImageIndex = 0;
            root.SelectedImageIndex = 0;

            // Phòng kỹ thuật
            TreeNode kyThuat = new TreeNode("Phòng Kỹ thuật");
            kyThuat.Tag = "KyThuat";
            kyThuat.ImageIndex = 1;
            kyThuat.SelectedImageIndex = 1;

            kyThuat.Nodes.Add(new TreeNode("Nhóm Phát triển")
            {
                Tag = "KyThuat|PhatTrien",
                ImageIndex = 2,
                SelectedImageIndex = 2
            });

            kyThuat.Nodes.Add(new TreeNode("Nhóm Kiểm thử")
            {
                Tag = "KyThuat|KiemThu",
                ImageIndex = 2,
                SelectedImageIndex = 2
            });

            // Phòng kế toán
            TreeNode keToan = new TreeNode("Phòng Kế toán");
            keToan.Tag = "KeToan";
            keToan.ImageIndex = 1;
            keToan.SelectedImageIndex = 1;

            keToan.Nodes.Add(new TreeNode("Nhóm Kế toán tổng hợp")
            {
                Tag = "KeToan|KeToanTongHop",
                ImageIndex = 2,
                SelectedImageIndex = 2
            });

            // Phòng nhân sự
            TreeNode nhanSu = new TreeNode("Phòng Nhân sự");
            nhanSu.Tag = "NhanSu";
            nhanSu.ImageIndex = 1;
            nhanSu.SelectedImageIndex = 1;

            nhanSu.Nodes.Add(new TreeNode("Nhóm Tuyển dụng")
            {
                Tag = "NhanSu|TuyenDung",
                ImageIndex = 2,
                SelectedImageIndex = 2
            });

            root.Nodes.Add(kyThuat);
            root.Nodes.Add(keToan);
            root.Nodes.Add(nhanSu);

            tvDepartments.Nodes.Add(root);

            // Mở rộng cây
            tvDepartments.ExpandAll();

            // Chọn công ty khi khởi chạy
            tvDepartments.SelectedNode = root;
        }

        // 4. Xử lý khi chọn Node trên TreeView
        private void TvDepartments_AfterSelect(
            object sender, TreeViewEventArgs e)
        {
            string key = e.Node.Tag.ToString();

            List<NhanVien> ketQua = new List<NhanVien>();

            if (key == "CongTy")
            {
                ketQua = danhSachNV;
            }
            else if (key.Contains("|"))
            {
                string[] parts = key.Split('|');

                ketQua = danhSachNV.Where(nv =>
                    nv.PhongBan == parts[0] &&
                    nv.Nhom == parts[1]).ToList();
            }
            else
            {
                ketQua = danhSachNV.Where(nv =>
                    nv.PhongBan == key).ToList();
            }

            HienThiNhanVien(ketQua);
        }

        // 5. Hiển thị nhân viên lên ListView
        private void HienThiNhanVien(List<NhanVien> ds)
        {
            lsvEmployees.Items.Clear();

            foreach (NhanVien nv in ds)
            {
                ListViewItem item = new ListViewItem(nv.MaNV);

                item.SubItems.Add(nv.HoTen);
                item.SubItems.Add(nv.ChucVu);
                item.SubItems.Add(
                    nv.NgayVaoLam.ToString("dd/MM/yyyy"));

                item.ImageIndex = 0;

                lsvEmployees.Items.Add(item);
            }

            // Tự động điều chỉnh độ rộng cột
            if (lsvEmployees.View == View.Details)
            {
                lsvEmployees.AutoResizeColumns(
                    ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        // 6. Chuyển đổi chế độ xem ListView
        private void CboView_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            switch (cboView.SelectedItem.ToString())
            {
                case "Details":
                    lsvEmployees.View = View.Details;
                    break;

                case "SmallIcon":
                    lsvEmployees.View = View.SmallIcon;
                    break;

                case "LargeIcon":
                    lsvEmployees.View = View.LargeIcon;
                    break;

                case "Tile":
                    lsvEmployees.View = View.Tile;
                    break;
            }
        }
    }
}
