
#process

* create bus
* initialise container
* load all addons
* build directed graph
* foreach addon
	* create instance
	* inject dependencies 	// just use ctor injection, or property injection, or service location?
* publish ApplicationInitializeMessage



#assembly contents

* common.dll
	- messages
		- DisplayDialogMessage.cs
		- DisplayLoginDialogMessage.cs
		- CreateMenuEntryMessage.cs
	- addon.cs


* assemblies:
	* candidates.dll
	* contacts.dll
	* vacancies.dll
	* navigation.dll
	* reports.dll
	* dataTools.dll
	* developer.dll

#classes


	public class Addon
	{

		protected void SubscribeTo<TMessage>()
		{}

		protected void Publish(object message)
		{}
	}



	public class DeveloperAddon
	{
		public DeveloperAddon()
		{
			CreateMenuEntry("Tools", "Developer Options", OnOptions)

			_permissions = Service.Permissions;

		}

		private void OnOptions(object sender, EventArgs e)
		{
			if (CurrentUser.IsDeveloper == false)
			{
				Publish(new displayLoginDialog(success => {
					if (_permissions.IsInGroup(success.User, Groups.ITSystems))
					{
						DisplayOptions();
					}
					else
					{
						Publish(new DisplayDialogMessage(
							() => MessageBox.Show("You must enter a developer account name.")
						))
					}
				});

				return;
			}

			DisplayOptions();
		}

		private void DisplayOptions()
		{
			Publish(new DisplayDialogMessage(
				() => new frmDeveloperOptions()
			));
		}
	}