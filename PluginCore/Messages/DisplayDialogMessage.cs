using System;
using System.Windows.Forms;

namespace PluginCore.Messages
{
	public class DisplayDialogMessage
	{
		private readonly Func<Form> _createDialog;
		private readonly Action<DialogResult> _onClose;

		public DisplayDialogMessage(Func<Form> createDialog, Action<DialogResult> onClose)
		{
			_createDialog = createDialog;
			_onClose = onClose;
		}

		public void Display()
		{
			Display(null);
		}

		public void Display(IWin32Window owner)
		{
			using (var dialog = _createDialog())
			{
				var result = dialog.ShowDialog(owner);

				_onClose(result);
			}
		}
	}
}
