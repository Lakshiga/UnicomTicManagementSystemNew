using System;
using System.Windows.Forms;
using UnicomTicManagementSystem.Controllers;
using UnicomTicManagementSystem.Models;

namespace UnicomTicManagementSystem.Views
{
    public partial class RoomForm : Form
    {
        private RoomController _roomController;
        private int selectedRoomId = -1;

        public RoomForm()
        {
            InitializeComponent();
            _roomController = new RoomController();
            LoadRooms();
        }

        private void LoadRooms()
        {
            dgvRooms.DataSource = _roomController.GetAllRooms();
            dgvRooms.ClearSelection();
        }     

        private void dgvRooms_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRooms.SelectedRows.Count > 0)
            {
                var room = dgvRooms.SelectedRows[0].DataBoundItem as Room;
                if (room != null)
                {
                    selectedRoomId = room.RoomID;
                    txtRoomName.Text = room.RoomName;
                    txtRoomType.Text = room.RoomType;
                }
            }
        }

        private void ClearFields()
        {
            selectedRoomId = -1;
            txtRoomName.Clear();
            txtRoomType.Clear();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            var room = new Room
            {
                RoomName = txtRoomName.Text.Trim(),
                RoomType = txtRoomType.Text.Trim()
            };

            _roomController.AddRoom(room);
            LoadRooms();
            ClearFields();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (selectedRoomId == -1)
            {
                MessageBox.Show("Please select a room to update.");
                return;
            }

            var room = new Room
            {
                RoomID = selectedRoomId,
                RoomName = txtRoomName.Text.Trim(),
                RoomType = txtRoomType.Text.Trim()
            };

            _roomController.UpdateRoom(room);
            LoadRooms();
            ClearFields();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (selectedRoomId == -1)
            {
                MessageBox.Show("Please select a room to delete.");
                return;
            }

            _roomController.DeleteRoom(selectedRoomId);
            LoadRooms();
            ClearFields();
        }
    }
}
