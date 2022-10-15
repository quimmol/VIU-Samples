package com.example.demo1;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.widget.Toast;

public class MyReceiver extends BroadcastReceiver {

    @Override
    public void onReceive(Context context, Intent intent) {

        Toast.makeText(context, "Received an intent.", Toast.LENGTH_SHORT).show();

        if (intent.getAction().equals(MapsActivity.ACTION1)) { //is it our action1?
            Toast.makeText(context, "Saludos!", Toast.LENGTH_SHORT).show();
        } else if (intent.getAction().equals(MapsActivity.ACTION2)) { //is it our action2?
            Toast.makeText(context, "Adios!", Toast.LENGTH_SHORT).show();
        }
    }
}