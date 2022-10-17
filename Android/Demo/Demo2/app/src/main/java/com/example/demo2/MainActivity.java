package com.example.demo2;

import androidx.appcompat.app.AppCompatActivity;
import androidx.localbroadcastmanager.content.LocalBroadcastManager;

import android.annotation.SuppressLint;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.Toast;

import java.io.BufferedInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

@SuppressWarnings("ALL")
public class MainActivity extends AppCompatActivity {

    ImageView mImageView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mImageView = (ImageView) findViewById(R.id.imageView);

        Button button = (Button) findViewById(R.id.button);
        button.setOnClickListener(v -> new RandomWallpaper().execute());
    }


    @Override
    protected void onStart() {
        super.onStart();

        Log.d("MainActivity", "Me estoy iniciando!");

        Intent intent = new Intent();
        intent.setAction("com.example.demo2.SOME_ACTION");
        sendBroadcast(intent);
    }

    @Override
    protected void onStop() {
        super.onStop();
        startService(new Intent( this, MyService.class ) );
    }

    @Override
    protected void onResume() {
        super.onResume();
        stopService(new Intent( this, MyService.class ) );

    }


    @SuppressLint("StaticFieldLeak")
    private class RandomWallpaper extends AsyncTask<URL,Void,Bitmap> {
        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Bitmap doInBackground(URL... params) {

            try {
                URL url = new URL("https://picsum.photos/800/800");
                HttpURLConnection connection;
                try {
                    connection = (HttpURLConnection) url.openConnection();
                    connection.connect();
                    InputStream inputStream = connection.getInputStream();
                    BufferedInputStream bufferedInputStream = new BufferedInputStream(inputStream);
                    return BitmapFactory.decodeStream(bufferedInputStream);
                } catch (IOException e) {
                    e.printStackTrace();
                }
            } catch (MalformedURLException e) {
                e.printStackTrace();
            }
            return null;
        }

        @Override
        protected void onPostExecute(Bitmap result) {
            if(result!=null)
            {
                mImageView.setImageBitmap(result);
                Toast.makeText(MainActivity.this, "Imagen cambiada!", Toast.LENGTH_SHORT).show();
            }
            else
            {
                Toast.makeText(MainActivity.this, "No hay internet?", Toast.LENGTH_SHORT).show();
            }
        }
    }

}


