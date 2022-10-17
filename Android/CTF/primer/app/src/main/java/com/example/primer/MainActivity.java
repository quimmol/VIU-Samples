package com.example.primer;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Base64;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.io.UnsupportedEncodingException;

public class MainActivity extends AppCompatActivity {


    private final String TAG = "MainActivity";

    private boolean loggedIn = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Log.d(TAG, "Hola Clase");

        Button button = findViewById(R.id.button);
        button.setOnClickListener(this::onClick);
    }

    private void onClick(View v) {
        TextView user = findViewById(R.id.user);
        TextView pass = findViewById(R.id.password);

        process_login(user.getText().toString().trim(), pass.getText().toString().trim());
    }

    protected void process_login(String user, String pass){

        String default_user = getString(R.string.user);
        byte[] test = Base64.decode(getString(R.string.secret),Base64.DEFAULT);

        String default_pass = null;
        try {
            default_pass = new String(test, "UTF-8");
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }

        if(default_user.equalsIgnoreCase(user) && default_pass.equalsIgnoreCase(pass)){
            Log.d(TAG, "Success");

            loggedIn = true;
            Intent intent = new Intent(MainActivity.this, SuccessActivity.class);
            startActivity(intent);
        }

    }

    @Override
    protected void onStop() {
        super.onStop();
        
        process_logout();
    }

    private void process_logout() {

        loggedIn = false;
    }

    @Override
    protected void onResume() {
        super.onResume();

        if(loggedIn){

            Intent intent = new Intent(MainActivity.this, SuccessActivity.class);
            startActivity(intent);
        }
    }
}