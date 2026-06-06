// Navigation/FormNavigator.cs
using inventory_of_equipment_in_classrooms.Forms;

namespace inventory_of_equipment_in_classrooms
{
    public static class FormNavigator
    {
        private static ProfileForm _profileForm;
        private static EditCardForm _editCardForm;
        private static TransferForm _transferForm;
        private static TransferActForm _transferActForm;
        private static ImportForm _importForm;

        private static int _currentUserId;

        public static void Init(int userId)
        {
            _currentUserId = userId;
        }

        public static void ShowProfile()
        {
            HideAll();
            if (_profileForm == null || _profileForm.IsDisposed)
                _profileForm = new ProfileForm(_currentUserId);
            _profileForm.Show();
        }

        public static void ShowEditCard()
        {
            HideAll();
            if (_editCardForm == null || _editCardForm.IsDisposed)
                _editCardForm = new EditCardForm(_currentUserId);
            _editCardForm.Show();
        }

        public static void ShowTransfer()
        {
            HideAll();
            if (_transferForm == null || _transferForm.IsDisposed)
                _transferForm = new TransferForm(_currentUserId);
            _transferForm.Show();
        }

        public static void ShowTransferAct()
        {
            HideAll();
            if (_transferActForm == null || _transferActForm.IsDisposed)
                _transferActForm = new TransferActForm(_currentUserId);
            _transferActForm.Show();
        }

        public static void ShowImport()
        {
            HideAll();
            if (_importForm == null || _importForm.IsDisposed)
                _importForm = new ImportForm(_currentUserId);
            _importForm.Show();
        }

        private static void HideAll()
        {
            _profileForm?.Hide();
            _editCardForm?.Hide();
            _transferForm?.Hide();
            _transferActForm?.Hide();
            _importForm?.Hide();
        }
    }
}