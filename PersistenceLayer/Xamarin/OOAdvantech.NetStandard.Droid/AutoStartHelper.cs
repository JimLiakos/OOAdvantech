﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace OOAdvantech.NetStandard.Droid
{
    class AutoStartHelpers
    {
    }
}


public class AutoStartHelper
{

    /***
     * Xiaomi
     */
    private String BRAND_XIAOMI = "xiaomi";
    private String PACKAGE_XIAOMI_MAIN = "com.miui.securitycenter";
    private String PACKAGE_XIAOMI_COMPONENT = "com.miui.permcenter.autostart.AutoStartManagementActivity";

    /***
     * Letv
     */
    private String BRAND_LETV = "letv";
    private String PACKAGE_LETV_MAIN = "com.letv.android.letvsafe";
    private String PACKAGE_LETV_COMPONENT = "com.letv.android.letvsafe.AutobootManageActivity";

    /***
     * ASUS ROG
     */
    private String BRAND_ASUS = "asus";
    private String PACKAGE_ASUS_MAIN = "com.asus.mobilemanager";
    private String PACKAGE_ASUS_COMPONENT = "com.asus.mobilemanager.powersaver.PowerSaverSettings";

    /***
     * Honor
     */
    private String BRAND_HONOR = "honor";
    private String PACKAGE_HONOR_MAIN = "com.huawei.systemmanager";
    private String PACKAGE_HONOR_COMPONENT = "com.huawei.systemmanager.optimize.process.ProtectActivity";

    /**
     * Oppo
     */
    private String BRAND_OPPO = "oppo";
    private String PACKAGE_OPPO_MAIN = "com.coloros.safecenter";
    private String PACKAGE_OPPO_FALLBACK = "com.oppo.safe";
    private String PACKAGE_OPPO_COMPONENT = "com.coloros.safecenter.permission.startup.StartupAppListActivity";
    private String PACKAGE_OPPO_COMPONENT_FALLBACK = "com.oppo.safe.permission.startup.StartupAppListActivity";
    private String PACKAGE_OPPO_COMPONENT_FALLBACK_A = "com.coloros.safecenter.startupapp.StartupAppListActivity";

    /**
     * Vivo
     */

    private String BRAND_VIVO = "vivo";
    private String PACKAGE_VIVO_MAIN = "com.iqoo.secure";
    private String PACKAGE_VIVO_FALLBACK = "com.vivo.perm;issionmanager";
    private String PACKAGE_VIVO_COMPONENT = "com.iqoo.secure.ui.phoneoptimize.AddWhiteListActivity";
    private String PACKAGE_VIVO_COMPONENT_FALLBACK = "com.vivo.permissionmanager.activity.BgStartUpManagerActivity";
    private String PACKAGE_VIVO_COMPONENT_FALLBACK_A = "com.iqoo.secure.ui.phoneoptimize.BgStartUpManager";

    /**
     * Nokia
     */

    private String BRAND_NOKIA = "nokia";
    private String PACKAGE_NOKIA_MAIN = "com.evenwell.powersaving.g3";
    private String PACKAGE_NOKIA_COMPONENT = "com.evenwell.powersaving.g3.exception.PowerSaverExceptionActivity";


    private AutoStartHelper()
    {
    }

    public static AutoStartHelper getInstance()
    {
        return new AutoStartHelper();
    }


    public void getAutoStartPermission(Context context)
    {

        String build_info = Build.BRAND.toLowerCase();
        switch (build_info)
        {
            case BRAND_ASUS:
                autoStartAsus(context);
                break;
            case BRAND_XIAOMI:
                autoStartXiaomi(context);
                break;
            case BRAND_LETV:
                autoStartLetv(context);
                break;
            case BRAND_HONOR:
                autoStartHonor(context);
                break;
            case BRAND_OPPO:
                autoStartOppo(context);
                break;
            case BRAND_VIVO:
                autoStartVivo(context);
                break;
            case BRAND_NOKIA:
                autoStartNokia(context);
                break;

        }

    }

    private void autoStartAsus( Context context)
    {
        if (isPackageExists(context, PACKAGE_ASUS_MAIN))
        {

            showAlert(context, (dialog, which)-> {
                try
                {
                    PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                    startIntent(context, PACKAGE_ASUS_MAIN, PACKAGE_ASUS_COMPONENT);
                }
                catch (Exception e)
                {
                    e.printStackTrace();
                }
                dialog.dismiss();
            });

        }


    }

    private void showAlert(Context context, DialogInterface.OnClickListener onClickListener)
    {

        new AlertDialog.Builder(context).setTitle("Allow AutoStart")
                .setMessage("Please enable auto start in settings.")
                .setPositiveButton("Allow", onClickListener).show().setCancelable(false);
    }

    private void autoStartXiaomi( Context context)
    {
        if (isPackageExists(context, PACKAGE_XIAOMI_MAIN))
        {
            showAlert(context, (dialog, which)-> {
                try
                {
                    PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                    startIntent(context, PACKAGE_XIAOMI_MAIN, PACKAGE_XIAOMI_COMPONENT);
                }
                catch (Exception e)
                {
                    e.printStackTrace();
                }
            });


        }
    }

    private void autoStartLetv( Context context)
    {
        if (isPackageExists(context, PACKAGE_LETV_MAIN))
        {
            showAlert(context, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which)
            {

                try
                {
                    PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                    startIntent(context, PACKAGE_LETV_MAIN, PACKAGE_LETV_COMPONENT);
                }
                catch (Exception e)
                {
                    e.printStackTrace();
                }
            }
        });


    }



    private void autoStartHonor( Context context)
    {
        if (isPackageExists(context, PACKAGE_HONOR_MAIN))
        {
            showAlert(context, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which)
            {

                try
                {
                    PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                    startIntent(context, PACKAGE_HONOR_MAIN, PACKAGE_HONOR_COMPONENT);
                }
                catch (Exception e)
                {
                    e.printStackTrace();
                }
            }
        });


    }


    private void autoStartOppo( Context context)
    {
        if (isPackageExists(context, PACKAGE_OPPO_MAIN) || isPackageExists(context, PACKAGE_OPPO_FALLBACK))
        {
            showAlert(context, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which)
            {

                try
                {
                    PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                    startIntent(context, PACKAGE_OPPO_MAIN, PACKAGE_OPPO_COMPONENT);
                }
                catch (Exception e)
                {
                    e.printStackTrace();
                    try
                    {
                        PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                        startIntent(context, PACKAGE_OPPO_FALLBACK, PACKAGE_OPPO_COMPONENT_FALLBACK);
                    }
                    catch (Exception ex)
                    {
                        ex.printStackTrace();
                        try
                        {
                            PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                            startIntent(context, PACKAGE_OPPO_MAIN, PACKAGE_OPPO_COMPONENT_FALLBACK_A);
                        }
                        catch (Exception exx)
                        {
                            exx.printStackTrace();
                        }

                    }

                }
            }
        });


    }


    private void autoStartVivo( Context context)
    {
        if (isPackageExists(context, PACKAGE_VIVO_MAIN) || isPackageExists(context, PACKAGE_VIVO_FALLBACK))
        {
            showAlert(context, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which)
            {

                try
                {
                    PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                    startIntent(context, PACKAGE_VIVO_MAIN, PACKAGE_VIVO_COMPONENT);
                }
                catch (Exception e)
                {
                    e.printStackTrace();
                    try
                    {
                        PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                        startIntent(context, PACKAGE_VIVO_FALLBACK, PACKAGE_VIVO_COMPONENT_FALLBACK);
                    }
                    catch (Exception ex)
                    {
                        ex.printStackTrace();
                        try
                        {
                            PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                            startIntent(context, PACKAGE_VIVO_MAIN, PACKAGE_VIVO_COMPONENT_FALLBACK_A);
                        }
                        catch (Exception exx)
                        {
                            exx.printStackTrace();
                        }

                    }

                }

            }
        });
    }


    private void autoStartNokia( Context context)
    {
        if (isPackageExists(context, PACKAGE_NOKIA_MAIN))
        {
            showAlert(context, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which)
            {

                try
                {
                    PrefUtil.writeBoolean(context, PrefUtil.PREF_KEY_APP_AUTO_START, true);
                    startIntent(context, PACKAGE_NOKIA_MAIN, PACKAGE_NOKIA_COMPONENT);
                }
                catch (Exception e)
                {
                    e.printStackTrace();
                }
            }
        });
    }



    private void startIntent(Context context, String packageName, String componentName)
    {
        try
        {
            Intent intent = new Intent();
            intent.SetComponent(new ComponentName(packageName, componentName));
            context.StartActivity(intent);
        }
        catch (Exception var5)
        {
            throw var5;
        }
    }

    private Boolean isPackageExists(Context context, String targetPackage)
    {
        List<Android.Content.PM.ApplicationInfo> packages;
        PackageManager pm = context.getPackageManager();
        packages = pm.getInstalledApplications(0);
        for (ApplicationInfo packageInfo :
            packages)
        {
            if (packageInfo.packageName.equals(targetPackage))
            {
                return true;
            }
        }

        return false;
    }
}