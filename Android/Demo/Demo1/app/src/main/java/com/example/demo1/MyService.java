package com.example.demo1;

import android.app.Service;
import android.content.Intent;
import android.os.Binder;
import android.os.IBinder;
import android.util.Log;
import android.widget.Toast;

public class MyService extends Service {
    MyBinder binder = new MyBinder();
    private String TAG = "MyService";

    public MyService() {
    }

    class MyBinder extends Binder {
        MyService getService() {
            return MyService.this;
        }
    }


    @Override
    public IBinder onBind(Intent intent) {

        try {
            Thread.sleep(3000);
            Toast.makeText(this, "Bienvenido al mundo de los servicios", Toast.LENGTH_LONG).show();
        } catch (InterruptedException e) {
            Log.e(TAG, e.toString());
        }
        return binder;
    }
}