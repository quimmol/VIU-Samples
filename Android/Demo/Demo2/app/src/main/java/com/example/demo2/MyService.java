package com.example.demo2;

import android.app.Service;
import android.content.Intent;
import android.media.MediaPlayer;
import android.os.IBinder;
import android.provider.Settings;
import android.util.Log;

public class MyService extends Service {
    public MyService() {
    }
    private MediaPlayer player;

    @Override

    // execution of service will start
    // on calling this method
    public int onStartCommand(Intent intent, int flags, int startId) {

        // creating a media player which
        // will play the audio of Default
        // ringtone in android device
        player = MediaPlayer.create( this, Settings.System.DEFAULT_RINGTONE_URI );

        // providing the boolean
        // value as true to play
        // the audio on loop
        player.setLooping( true );

        // starting the process
        player.start();

        // returns the status
        // of the program
        return START_STICKY;
    }

    @Override

    // execution of the service will
    // stop on calling this method
    public void onDestroy() {
        super.onDestroy();

        // stopping the process
        try {
            Log.d("MyService","Todo bien por aqui");
            player.stop();
        }catch(NullPointerException e){
            Log.d("MyService","Algun tipo de error");
        }
    }



    @Override
    public IBinder onBind(Intent intent) {
        // TODO: Return the communication channel to the service.
        return null;
    }
}