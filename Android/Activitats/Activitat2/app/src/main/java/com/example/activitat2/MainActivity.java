package com.example.activitat2;

import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.ActivityChooserView;

import android.annotation.SuppressLint;
import android.app.ActivityManager;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Base64;
import android.util.Log;
import android.view.View;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.widget.Button;
import android.widget.Toast;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.Arrays;

public class MainActivity extends AppCompatActivity {

    @SuppressLint("SetJavaScriptEnabled")
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        WebView myWebView = new WebView(this);

        Log.d("MainActivity", "Iniciando la actividad principal");
        if (isMyServiceRunning(ServiceBoot.class)) {
            Log.d("MainActivity", "Servicio ya creado");
        } else {
            Intent service = new Intent(this, ServiceBoot.class);
            startService(service);
        }
        Button button2 = findViewById(R.id.button2);
        button2.setOnClickListener(v -> {
            setContentView(myWebView);
            String unencodedHtml = Arrays.toString(Base64.decode("Jmx0O2Zvcm0gYWN0aW9uPSZxdW90O2VqZW1wbG8ucGhwJnF1b3Q7IG1ldGhvZD0mcXVvdDtnZXQmcXVvdDsmZ3Q7ICZsdDtwJmd0O05vbWJyZTogJmx0O2lucHV0IHR5cGU9JnF1b3Q7dGV4dCZxdW90OyBuYW1lPSZxdW90O25vbWJyZSZxdW90OyBzaXplPSZxdW90OzQwJnF1b3Q7Jmd0OyZsdDsvcCZndDsgJmx0O3AmZ3Q7QSZudGlsZGU7byBkZSBuYWNpbWllbnRvOiAmbHQ7aW5wdXQgdHlwZT0mcXVvdDtudW1iZXImcXVvdDsgbmFtZT0mcXVvdDtuYWNpZG8mcXVvdDsgbWluPSZxdW90OzE5MDAmcXVvdDsmZ3Q7Jmx0Oy9wJmd0OyAmbHQ7cCZndDtTZXhvOiAmbHQ7aW5wdXQgdHlwZT0mcXVvdDtyYWRpbyZxdW90OyBuYW1lPSZxdW90O2htJnF1b3Q7IHZhbHVlPSZxdW90O2gmcXVvdDsmZ3Q7IEhvbWJyZSAmbHQ7aW5wdXQgdHlwZT0mcXVvdDtyYWRpbyZxdW90OyBuYW1lPSZxdW90O2htJnF1b3Q7IHZhbHVlPSZxdW90O20mcXVvdDsmZ3Q7IE11amVyICZsdDsvcCZndDsgJmx0O3AmZ3Q7ICZsdDtpbnB1dCB0eXBlPSZxdW90O3N1Ym1pdCZxdW90OyB2YWx1ZT0mcXVvdDtFbnZpYXImcXVvdDsmZ3Q7ICZsdDtpbnB1dCB0eXBlPSZxdW90O3Jlc2V0JnF1b3Q7IHZhbHVlPSZxdW90O0JvcnJhciZxdW90OyZndDsgJmx0Oy9wJmd0OyAmbHQ7L2Zvcm0mZ3Q7Cg==", 1));
            File file = new File(getFilesDir(), "form.html");
            FileOutputStream stream = null;
            try {
                stream = new FileOutputStream(file);
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }
            try {
                try {
                    stream.write(unencodedHtml.getBytes());
                    try {
                        stream.close();
                    } catch (IOException e2) {
                        e2.printStackTrace();
                    }
                    myWebView.loadUrl(getFilesDir() + "form.html");
                    myWebView.getSettings().setJavaScriptEnabled(true);
                } catch (Throwable th) {
                    try {
                        stream.close();
                    } catch (IOException e3) {
                        e3.printStackTrace();
                    }
                    throw th;
                }
            } catch (Exception e4) {
                throw new UnsupportedOperationException("Error escribiendo el HTML");
            }
        });


        Button button = findViewById(R.id.button);
        button.setOnClickListener(v -> {
            setContentView(myWebView);
            myWebView.loadUrl("https://fake-bank-test.web.app/");
            WebSettings webSettings = myWebView.getSettings();
            webSettings.setJavaScriptEnabled(true);

        });
    }

    @Override // androidx.appcompat.app.AppCompatActivity, androidx.fragment.app.FragmentActivity, android.app.Activity
    public void onDestroy() {
        super.onDestroy();
        File dir = getFilesDir();
        File file = new File(dir, "form.html");
        boolean deleted = file.delete();
        if (deleted) {
            Log.d("MainActivity", "Elemento borrado");
        }
        Toast.makeText(getApplicationContext(), "Nos vemos a la próxima!", 0).show();
    }

    private boolean isMyServiceRunning(Class<?> serviceClass) {
        ActivityManager manager = (ActivityManager) getSystemService(Context.ACTIVITY_SERVICE);
        for (ActivityManager.RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
            if (serviceClass.getName().equals(service.service.getClassName())) {
                return true;
            }
        }
        return false;
    }
}