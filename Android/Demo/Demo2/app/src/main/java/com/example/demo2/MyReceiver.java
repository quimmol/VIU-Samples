package com.example.demo2;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;

import java.net.InetAddress;
import java.net.NetworkInterface;
import java.net.SocketException;
import java.util.Enumeration;

public class MyReceiver extends BroadcastReceiver {

    @Override
    public void onReceive(Context context, Intent intent) {

       String result = intent.getAction();

       Log.d("MyReceiver",result);

       switch (result)
       {
           case "com.example.demo2.SOME_ACTION":
               Toast.makeText(context, "Estas haciendo algo?", Toast.LENGTH_LONG).show();
           case "com.example.demo2.GET_IP":
               Toast.makeText(context, getMobileIP(), Toast.LENGTH_LONG).show();

               Log.d("MyReceiver",getMobileIP());
           default:
               Toast.makeText(context, "LONG LIVE RICK & MORTY", Toast.LENGTH_LONG).show();

       }
    }

    public static String getMobileIP() {
        try {
            for (Enumeration<NetworkInterface> en = NetworkInterface
                    .getNetworkInterfaces(); en.hasMoreElements();) {
                NetworkInterface intf = en.nextElement();
                for (Enumeration<InetAddress> enumIpAddr = intf
                        .getInetAddresses(); enumIpAddr.hasMoreElements();) {
                    InetAddress inetAddress = enumIpAddr.nextElement();
                    if (!inetAddress.isLoopbackAddress()) {
                        return inetAddress .getHostAddress();
                    }
                }
            }
        } catch (SocketException ex) {
            Log.e("MyReceiver", "Exception in Get IP Address: " + ex);
        }
        return null;
    }


}