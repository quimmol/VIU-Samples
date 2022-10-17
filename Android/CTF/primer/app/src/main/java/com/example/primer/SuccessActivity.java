package com.example.primer;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.webkit.WebView;

public class SuccessActivity extends AppCompatActivity {

    private static final String TAG = "SuccessActivity";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_success);


        Log.d(TAG, "Enhorabuena!");
        WebView mWebView = findViewById(R.id.webview);
        mWebView.getSettings().setJavaScriptEnabled(true);
        mWebView.loadUrl("https://www.youtube.com/watch?v=mtFcKbqR-b81");
    }
}