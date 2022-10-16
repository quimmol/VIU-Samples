package com.example.demo1;

import androidx.appcompat.app.AppCompatActivity;

import android.content.ComponentName;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.Bundle;
import android.os.IBinder;
import android.util.Log;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {

    Intent intent;
    MyService service;

    ServiceConnection sConn;

    private String TAG = "MapsActivity";

    public static final String ACTION1 = "viu.saludos";
    public static final String ACTION2 = "viu.adios";

    MyReceiver myReceiver;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Toast.makeText(this, "Hola clase!", Toast.LENGTH_SHORT).show();


        intent = new Intent(this, MyService.class);

        sConn = new ServiceConnection() {
            public void onServiceConnected(ComponentName name, IBinder binder) {
                Log.i(TAG, "MainActivity onServiceConnected");
                service = ((MyService.MyBinder) binder).getService();

                Intent i = new Intent(MainActivity.ACTION1);
                i.setPackage("viu.saludos");
                sendBroadcast(i);

            }

            @Override
            public void onServiceDisconnected(ComponentName componentName) {
                Intent i = new Intent(MainActivity.ACTION2);
                i.setPackage("viu.adios");
                sendBroadcast(i);
            }
        };
    }
}