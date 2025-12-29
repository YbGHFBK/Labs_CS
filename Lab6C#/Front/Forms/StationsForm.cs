using CustomControls;
using System.Drawing;

public class StationsForm : Form
{
    private string country;
    private string city;

    private FlowLayoutPanel fpList;
    private RoundedFlowLayoutPanel mainPanel;

    private Station editingStation = null!;

    private RoundedTextBox tbCountry;
    private RoundedTextBox tbCity;
    private DropDownRoundedButton btnSave;

    public StationsForm()
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
            Size = new Size(600, 450),
            Location = new Point(660, 300),
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderSize = 1,
            BorderRadius = 18,
            Padding = new Padding(40),
            Visible = false
        };
        Controls.Add(mainPanel);

        mainPanel.Controls.Add(new Label { Text = "Country", Font = new Font("Segoe UI", 12f), AutoSize = true });
        tbCountry = new RoundedTextBox { Width = 520, Margin = new Padding(0, 0, 0, 20) };
        mainPanel.Controls.Add(tbCountry);

        mainPanel.Controls.Add(new Label { Text = "City", Font = new Font("Segoe UI", 12f), AutoSize = true });
        tbCity = new RoundedTextBox { Width = 520, Margin = new Padding(0, 0, 0, 30) };
        mainPanel.Controls.Add(tbCity);

        btnSave = new DropDownRoundedButton
        {
            Size = new Size(200, 45),
            ButtonText = "Save Station",
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
            ButtonText = "+ Add New Station",
            Font = new Font("Segoe UI", 12f),
            BackColor = Color.White,
            BorderColor = Color.LightGray,
            BorderRadius = 10,
            Margin = new Padding(0, 0, 0, 20)
        };
        fpList.Controls.Add(btnAdd);

        btnAdd.Click += (s, e) =>
        {
            editingStation = null;
            tbCountry.TbText = "";
            tbCity.TbText = "";
            btnSave.ButtonText = "Save Station";
            fpList.Visible = false;
            mainPanel.Visible = true;
        };

        btnSave.Click += (s, e) =>
        {
            if (!string.IsNullOrWhiteSpace(tbCountry.TbText) && !string.IsNullOrWhiteSpace(tbCity.TbText))
            {
                if (editingStation == null)
                {
                    DB.createStation(tbCountry.TbText, tbCity.TbText);
                }
                else
                {
                    editingStation.country = tbCountry.TbText;
                    editingStation.city = tbCity.TbText;
                    editingStation = null;
                }

                tbCountry.TbText = "";
                tbCity.TbText = "";
                mainPanel.Visible = false;
                fpList.Visible = true;
                RefreshStationList();
            }
            else
            {
                MessageBox.Show("Please fill in all fields");
            }
        };

        RefreshStationList();
    }

    private void GoToMain()
    {
        var main = new MainPageForm(1920, 1080);
        main.Show();
        Close();
    }

    private void RefreshStationList()
    {
        while (fpList.Controls.Count > 1)
        {
            fpList.Controls.RemoveAt(1);
        }

        var stations = DB.stations;
        stations.Sort();

        foreach (var st in stations)
        {
            var stPanel = new StationItemPanel(st);

            stPanel.DeleteRequested += (stationToDelete) => {
                var res = MessageBox.Show($"Удалить станцию {stationToDelete}?", "Подтверждение", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    DB.DeleteStation(stationToDelete);
                    RefreshStationList();
                }
            };

            stPanel.EditRequested += (stationToEdit) => {
                editingStation = stationToEdit;
                tbCountry.TbText = stationToEdit.country;
                tbCity.TbText = stationToEdit.city;

                btnSave.ButtonText = "Обновить данные";
                DB.Save();
                fpList.Visible = false;
                mainPanel.Visible = true;
            };

            fpList.Controls.Add(stPanel);
        }
    }
}

public class StationItemPanel : UserControl
{
    public event Action<Station> EditRequested;
    public event Action<Station> DeleteRequested;

    public StationItemPanel(Station station)
    {
        Size = new Size(1550, 60);
        BackColor = Color.White;
        Margin = new Padding(0, 5, 0, 5);

        var lbl = new Label
        {
            Text = station.ToString(),
            Font = new Font("Segoe UI", 12f),
            Location = new Point(20, 18),
            AutoSize = true
        };

        var btnDelete = new Button
        {
            Text = "Удалить",
            Size = new Size(100, 35),
            Location = new Point(1430, 12),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(254, 242, 242),
            ForeColor = Color.FromArgb(220, 38, 38),
            Cursor = Cursors.Hand
        };
        btnDelete.FlatAppearance.BorderColor = Color.FromArgb(220, 38, 38);
        btnDelete.Click += (s, e) => DeleteRequested?.Invoke(station);

        var btnEdit = new Button
        {
            Text = "Изменить",
            Size = new Size(100, 35),
            Location = new Point(1320, 12),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(240, 249, 255),
            ForeColor = Color.FromArgb(2, 132, 199),
            Cursor = Cursors.Hand
        };
        btnEdit.FlatAppearance.BorderColor = Color.FromArgb(2, 132, 199);
        btnEdit.Click += (s, e) => EditRequested?.Invoke(station);

        Controls.Add(lbl);
        Controls.Add(btnEdit);
        Controls.Add(btnDelete);
    }
}