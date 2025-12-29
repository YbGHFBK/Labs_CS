using CustomControls;
using System.Drawing.Drawing2D;

public class TrainsForm : Form
{
    private IMessageFilter menuFilterType;
    private IMessageFilter menuFilterCondition;

    private TrainType SelectedType;
    private TrainCondition SelectedCondition;
    private Train editingTrain = null!;

    private FlowLayoutPanel fpList;
    private RoundedFlowLayoutPanel mainPanel;
    private RoundedTextBox tbModel;
    private RoundedTextBox tbMileage;
    private DropDownRoundedButton btnSave;
    private DropDownRoundedButton btnType;
    private DropDownRoundedButton btnCond;

    private class ClickOutsideMenuFilter : IMessageFilter
    {
        private readonly Control menu;
        private readonly Control button;

        public ClickOutsideMenuFilter(Control menu, Control button)
        {
            this.menu = menu;
            this.button = button;
        }

        public bool PreFilterMessage(ref Message m)
        {
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_RBUTTONDOWN = 0x0204;

            if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_RBUTTONDOWN)
            {
                if (!menu.Visible) return false;
                Point p = Control.MousePosition;
                bool clickOnMenu = menu.RectangleToScreen(menu.ClientRectangle).Contains(p);
                bool clickOnButton = button.RectangleToScreen(button.ClientRectangle).Contains(p);
                if (!clickOnMenu && !clickOnButton) menu.Hide();
            }
            return false;
        }
    }

    public TrainsForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(1920, 1080);
        BackColor = Color.FromArgb(245, 245, 245);

        var header = new Header(1920, this)
        {
            Dock = DockStyle.Top,
            Height = 65,
            BackColor = Color.White
        };
        header.LogoClicked += () => GoToMain();
        Controls.Add(header);

        mainPanel = new RoundedFlowLayoutPanel
        {
            Size = new Size(600, 750),
            Location = new Point(660, 150),
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderSize = 1,
            BorderRadius = 18,
            Padding = new Padding(40),
            Visible = false
        };
        Controls.Add(mainPanel);

        mainPanel.Controls.Add(new Label { Text = "Train Model", Font = new Font("Segoe UI", 12f), AutoSize = true });
        tbModel = new RoundedTextBox { Width = 520, Margin = new Padding(0, 0, 0, 20) };
        mainPanel.Controls.Add(tbModel);

        mainPanel.Controls.Add(new Label { Text = "Train Type", Font = new Font("Segoe UI", 12f), AutoSize = true });
        btnType = new DropDownRoundedButton
        {
            Size = new Size(520, 40),
            ButtonText = "Select Type",
            Font = new Font("Segoe UI", 12f),
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Margin = new Padding(0, 0, 0, 20)
        };
        var menuType = new AutoSizedMenuPanel { Width = 520, Visible = false, BorderRadius = 10, BorderSize = 1, BorderColor = Color.LightGray };
        Controls.Add(menuType);
        mainPanel.Controls.Add(btnType);

        btnType.Click += (s, e) =>
        {
            if (menuType.Visible) { menuType.Hide(); return; }
            menuType.Clear();
            foreach (TrainType t in Enum.GetValues(typeof(TrainType)))
            {
                menuType.AddItem(t.ToString(), () => {
                    SelectedType = t;
                    btnType.ButtonText = t.ToString();
                    menuType.Hide();
                });
            }
            menuType.Location = mainPanel.PointToScreen(new Point(btnType.Left, btnType.Bottom));
            menuType.Visible = true;
            menuType.BringToFront();
            if (menuFilterType == null) menuFilterType = new ClickOutsideMenuFilter(menuType, btnType);
            Application.AddMessageFilter(menuFilterType);
        };

        mainPanel.Controls.Add(new Label { Text = "Condition", Font = new Font("Segoe UI", 12f), AutoSize = true });
        btnCond = new DropDownRoundedButton
        {
            Size = new Size(520, 40),
            ButtonText = "Select Condition",
            Font = new Font("Segoe UI", 12f),
            BackColor = Color.White,
            BorderColor = Color.FromArgb(250, 204, 206),
            BorderRadius = 10,
            BorderSize = 1,
            Margin = new Padding(0, 0, 0, 20)
        };
        var menuCond = new AutoSizedMenuPanel { Width = 520, Visible = false, BorderRadius = 10, BorderSize = 1, BorderColor = Color.LightGray };
        Controls.Add(menuCond);
        mainPanel.Controls.Add(btnCond);

        btnCond.Click += (s, e) =>
        {
            if (menuCond.Visible) { menuCond.Hide(); return; }
            menuCond.Clear();
            foreach (TrainCondition c in Enum.GetValues(typeof(TrainCondition)))
            {
                menuCond.AddItem(c.ToString(), () => {
                    SelectedCondition = c;
                    btnCond.ButtonText = c.ToString();
                    menuCond.Hide();
                });
            }
            menuCond.Location = mainPanel.PointToScreen(new Point(btnCond.Left, btnCond.Bottom));
            menuCond.Visible = true;
            menuCond.BringToFront();
            if (menuFilterCondition == null) menuFilterCondition = new ClickOutsideMenuFilter(menuCond, btnCond);
            Application.AddMessageFilter(menuFilterCondition);
        };

        mainPanel.Controls.Add(new Label { Text = "Mileage (km)", Font = new Font("Segoe UI", 12f), AutoSize = true });
        tbMileage = new RoundedTextBox { Width = 520, Margin = new Padding(0, 0, 0, 30) };
        mainPanel.Controls.Add(tbMileage);

        btnSave = new DropDownRoundedButton
        {
            Size = new Size(200, 45),
            ButtonText = "Save Train",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            NormalBackColor = Color.FromArgb(220, 38, 38),
            ForeColor = Color.White,
            BorderRadius = 12,
            BorderSize = 0
        };
        mainPanel.Controls.Add(btnSave);

        fpList = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            Size = new Size(1600, 860),
            Location = new Point(160, 220),
            AutoScroll = true
        };
        Controls.Add(fpList);

        var btnAdd = new DropDownRoundedButton
        {
            Size = new Size(300, 45),
            ButtonText = "+ Add New Train",
            Font = new Font("Segoe UI", 12f),
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderRadius = 10,
            Margin = new Padding(0, 0, 0, 20)
        };
        fpList.Controls.Add(btnAdd);

        btnAdd.Click += (s, e) =>
        {
            editingTrain = null!;
            tbModel.TbText = "";
            tbMileage.TbText = "";
            btnType.ButtonText = "Select Type";
            btnCond.ButtonText = "Select Condition";
            btnSave.ButtonText = "Save Train";
            fpList.Visible = false;
            mainPanel.Visible = true;
        };

        btnSave.Click += (s, e) =>
        {
            if (!string.IsNullOrWhiteSpace(tbModel.TbText) && int.TryParse(tbMileage.TbText, out int mileage))
            {
                if (editingTrain == null)
                {
                    DB.createTrain(tbModel.TbText, SelectedType, SelectedCondition, mileage);
                }
                else
                {
                    editingTrain.model = tbModel.TbText;
                    editingTrain.type = SelectedType;
                    editingTrain.condition = SelectedCondition;
                    editingTrain.mileage = mileage;
                    editingTrain = null!;
                }

                mainPanel.Visible = false;
                fpList.Visible = true;
                RefreshTrainList();
            }
            else
            {
                MessageBox.Show("Please fill in all fields correctly");
            }
        };

        RefreshTrainList();
    }

    private void GoToMain()
    {
        var main = new MainPageForm(1920, 1080);
        main.Show();
        Close();
    }

    private void RefreshTrainList()
    {
        while (fpList.Controls.Count > 1)
        {
            fpList.Controls.RemoveAt(1);
        }

        foreach (var t in DB.trains)
        {
            var tPanel = new TrainPanel(t);

            tPanel.DeleteRequested += (trainToDelete) => {
                var res = MessageBox.Show($"Удалить поезд {trainToDelete.model}?", "Подтверждение", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    DB.DeleteTrain(trainToDelete);
                    RefreshTrainList();
                }
            };

            tPanel.EditRequested += (trainToEdit) => {
                editingTrain = trainToEdit;
                tbModel.TbText = trainToEdit.model;
                tbMileage.TbText = trainToEdit.mileage.ToString();
                SelectedType = trainToEdit.type;
                SelectedCondition = trainToEdit.condition;
                btnType.ButtonText = trainToEdit.type.ToString();
                btnCond.ButtonText = trainToEdit.condition.ToString();

                btnSave.ButtonText = "Обновить данные";
                DB.Save();
                fpList.Visible = false;
                mainPanel.Visible = true;
            };

            fpList.Controls.Add(tPanel);
        }
    }
}

public class TrainPanel : UserControl
{
    public event Action<Train> EditRequested;
    public event Action<Train> DeleteRequested;
    private Train _train;
    private int borderRadius = 18;

    public TrainPanel(Train train)
    {
        _train = train;
        this.DoubleBuffered = true;
        this.Size = new Size(1550, 140);
        this.BackColor = Color.White;
        this.Margin = new Padding(0, 0, 0, 15);

        var btnDelete = new Button
        {
            Text = "Удалить",
            Size = new Size(100, 35),
            Location = new Point(1430, 85),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(254, 242, 242),
            ForeColor = Color.FromArgb(220, 38, 38),
            Cursor = Cursors.Hand
        };
        btnDelete.FlatAppearance.BorderColor = Color.FromArgb(220, 38, 38);
        btnDelete.Click += (s, e) => DeleteRequested?.Invoke(_train);

        var btnEdit = new Button
        {
            Text = "Изменить",
            Size = new Size(100, 35),
            Location = new Point(1320, 85),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(240, 249, 255),
            ForeColor = Color.FromArgb(2, 132, 199),
            Cursor = Cursors.Hand
        };
        btnEdit.FlatAppearance.BorderColor = Color.FromArgb(2, 132, 199);
        btnEdit.Click += (s, e) => EditRequested?.Invoke(_train);

        var btnSee = new DropDownRoundedButton
        {
            Size = new Size(210, 45),
            Location = new Point(1320, 25),
            ButtonText = "See Carrieges",
            NormalBackColor = Color.White,
            ForeColor = Color.Crimson,
            BorderRadius = 12,
            BorderSize = 1,
            BorderColor = Color.Crimson
        };
        btnSee.Click += (s, e) => {
            var carForm = new CarriegesForm(_train);
            carForm.Show();
            this.FindForm().Hide();
        };

        Controls.Add(btnSee);
        Controls.Add(btnEdit);
        Controls.Add(btnDelete);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using (GraphicsPath path = GetRoundPath(rect, borderRadius))
        {
            this.Region = new Region(path);
            g.DrawPath(Pens.LightGray, path);
        }

        Font fBold = new Font("Segoe UI", 14f, FontStyle.Bold);
        Font fReg = new Font("Segoe UI", 11f);

        g.DrawString($"{_train.model} (ID: {_train.Id})", fBold, Brushes.Black, 25, 30);
        g.DrawString($"Type: {_train.type}", fReg, Brushes.Gray, 25, 70);
        g.DrawString($"Condition: {_train.condition}", fReg, Brushes.Gray, 250, 70);
        g.DrawString($"Mileage: {_train.mileage} km", fReg, Brushes.Gray, 500, 70);
        g.DrawString($"Carriages: {_train.carrieges.Count}", fReg, Brushes.Gray, 750, 70);
    }

    private GraphicsPath GetRoundPath(Rectangle r, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}