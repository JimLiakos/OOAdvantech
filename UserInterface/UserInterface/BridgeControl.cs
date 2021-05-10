namespace OOAdvantech.UserInterface
{
	/// <MetaDataID>{494CCFA2-05B1-4BE1-958E-15DFC794A700}</MetaDataID>
	public class BridgeControl
	{

		public static void OnComponentAdded(object sender,	System.ComponentModel.Design.ComponentEventArgs e)
		{
			if(MapedControls.Contains(e.Component))
				return;


			int gg=0;
		}

		public static void OnComponentRemoved(object sender,System.ComponentModel.Design.ComponentEventArgs e)
		{
			int gg=0;
		}

		/// <MetaDataID>{7781AD66-D341-49D2-981E-31863E4BE37E}</MetaDataID>
		public static void ComponentChangedEventHandler(object sender,System.ComponentModel.Design.ComponentChangedEventArgs e)
		{
			
			if(MapedControls.Contains(e.Component))
			{
				OOAdvantech.UserInterface.Control control=MapedControls[e.Component] as OOAdvantech.UserInterface.Control;
				using( OOAdvantech.Transactions.ObjectStateTransition stateTransition=new OOAdvantech.Transactions.ObjectStateTransition(control))
				{
					if(e.Member.Name=="Controls")
					{
						System.Windows.Forms.Control.ControlCollection newControlCollection=e.NewValue as System.Windows.Forms.Control.ControlCollection;
						foreach(System.Windows.Forms.Control Childcontrol in newControlCollection )
						{
							if(MapedControls.Contains(Childcontrol))
								continue;
							BridgeControl bridgeControl=new BridgeControl(Childcontrol as object,MapedControls[e.Component] as ContainerControl );
						}
						
					}
					control.Size=(e.Component as System.Windows.Forms.Control).Size;
					control.Location=(e.Component as System.Windows.Forms.Control).Location;
					stateTransition.Consistent=true;;
				}
			}
			int t=0;
		}
		/// <MetaDataID>{7D8B7917-E548-40F8-B195-8CF42A8BE25A}</MetaDataID>
		public static OOAdvantech.Collections.Map MapedControls=new OOAdvantech.Collections.Map(10);
		
		public BridgeControl(object nativeControl,ContainerControl containerControl)
		{
			Control newControl=null;
			using(OOAdvantech.Transactions.ObjectStateTransition stateTransition=new OOAdvantech.Transactions.ObjectStateTransition(containerControl))
			{
                
				if(nativeControl is System.Windows.Forms.TextBox)
				{
					
					OOAdvantech.UserInterface.TextBox textBox=OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(containerControl).NewObject(typeof(OOAdvantech.UserInterface.TextBox)) as OOAdvantech.UserInterface.TextBox;
					newControl=textBox;
					newControl.Location=(nativeControl as System.Windows.Forms.Control).Location;
					newControl.Size=(nativeControl as System.Windows.Forms.Control).Size;
				}
				else
					return;// error prone

				containerControl.Controls.Add(newControl);
				stateTransition.Consistent=true;;
			}
			NativeControl=nativeControl;
			MapedControls.Add(NativeControl,newControl);
			newControl.Implementation=this;

		}
		/// <MetaDataID>{E67B3976-F4BE-4DBF-927C-6FAA3AD3BA8A}</MetaDataID>
		 public BridgeControl(Control control, System.ComponentModel.Design.IDesignerHost designerHost)
		{
			if(control is ObjectView)
			{
				System.Windows.Forms.Form form=designerHost.CreateComponent(typeof(System.Windows.Forms.Form)) as System.Windows.Forms.Form;
				form.Size=control.Size;
				NativeControl=form;
				foreach(Control Childcontrol in (control as ContainerControl).Controls)
				{
					BridgeControl bridgeControl=new BridgeControl(Childcontrol,designerHost);
					form.Controls.Add(bridgeControl.NativeControl as System.Windows.Forms.Control);
				}
			}
			if(control is TextBox)
			{
				System.Windows.Forms.TextBox textBox=designerHost.CreateComponent(typeof(System.Windows.Forms.TextBox)) as System.Windows.Forms.TextBox;
				textBox.Size=control.Size;
				textBox.Location=control.Location;
				NativeControl=textBox;
			}
			MapedControls.Add(NativeControl,control);
			control.Implementation=this;
		}
		/// <MetaDataID>{EF681022-C0AB-49A2-8EA6-03EBF5E8F268}</MetaDataID>
		public object NativeControl;
	}
}
