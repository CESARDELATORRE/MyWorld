package md54694411a32b46031a3becc66a57575c3;


public abstract class RunnerActivity
	extends md5b60ffeb829f638581ab2bb9b1a7f4f3f.FormsApplicationActivity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Xunit.Runners.UI.RunnerActivity, xunit.runner.devices, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null", RunnerActivity.class, __md_methods);
	}


	public RunnerActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == RunnerActivity.class)
			mono.android.TypeManager.Activate ("Xunit.Runners.UI.RunnerActivity, xunit.runner.devices, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
