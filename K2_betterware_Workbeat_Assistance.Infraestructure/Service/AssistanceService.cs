﻿using K2_Betterware_Workbeat_Assistance.Core.DTOs;
using K2_Betterware_Workbeat_Assistance.Infraestructure.ExternaService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
//using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json.Serialization;
using System.Net.Http;

namespace K2_Betterware_Workbeat_Assistance.Infraestructure.Service
{
    public class AssistanceService
    {

        public string getToken()
        {
            string srcv = "https://api.workbeat.com/oauth/token";
            string meth = "POST";
            string usr_access = "grant_type=client_credentials&client_id=7E800F92-B08E-4A12-9C5C-EFB03E170301&client_secret=4088828B-9601-400F-BDDC-EA73818BA4C4";
            ////////////////////////////////////////////////////////////////////////////////
            string cont_type = "application/x-www-form-urlencoded";
            string respuesta_tok = WorkBeatConnection.posting(srcv, usr_access, meth, cont_type);
            //////////////////////////////// descerializando cadenas json /////////////////////////////////
            Token from_js = JsonSerializer.Deserialize<Token>(respuesta_tok);
            Console.WriteLine(from_js.access_token);
            ////////////////////////////////////////////////////////////////////////////////////////////////
            return from_js.access_token;
        }


        public string giveme_workbeat_generic(string servapi)
        {
            string usr_access = getToken();

            var myRequest = (HttpWebRequest)WebRequest.Create("https://api.workbeat.com" + servapi + "?access_token=" + getToken());
            myRequest.Method = "GET";
            WebResponse response = myRequest.GetResponse();
            Stream strReader = response.GetResponseStream();
            StreamReader objReader = new StreamReader(strReader);
            string responseBody = objReader.ReadToEnd();
            return responseBody;
        }


        public string giveme_workbeat_empleados(string servapi)
        {

            string usr_access = getToken();
            var myRequest = (HttpWebRequest)WebRequest.Create("https://api.workbeat.com" + servapi + "?access_token=" + getToken());
            myRequest.Method = "GET";
            WebResponse response = myRequest.GetResponse();
            Stream strReader = response.GetResponseStream();
            StreamReader objReader = new StreamReader(strReader);
            string responseBody = objReader.ReadToEnd();
            return responseBody;
        }


        public string giveme_workbeat_persona(string servapi, string npr)
        {
            string usr_access = getToken();
            var myRequest = (HttpWebRequest)WebRequest.Create("https://api.workbeat.com" + servapi + npr + "?access_token=" + getToken());
            myRequest.Method = "GET";
            WebResponse response = myRequest.GetResponse();
            Stream strReader = response.GetResponseStream();
            StreamReader objReader = new StreamReader(strReader);
            string responseBody = objReader.ReadToEnd();
            return responseBody;
        }

        public string Checando(string p_id, string fechahora, string dispositivoId, string posi)
        {
            string token = getToken();
            //string direccion = "https: //api.workbeat.com/v2/asi/checada?id=8251&fechahora=2019-11-05T09:04:08&dispositivoId=11948&dirección=[E|1|N]";
            string direccion = "https://api.workbeat.com/v2/asi/checada?id=" + p_id + "&fechahora=" + fechahora + "&dispositivoId=" + dispositivoId + "&dirección=" + posi;
            string responseBody = "nada";
            var url = direccion;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("Accept", "application/json");
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) ;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            Console.WriteLine(responseBody);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                responseBody = ex.ToString();
            }
            return responseBody;
        }

        ////////////////////////////// metodos biostar /////////////////////////////////////////////////////////////
        /*public string token_bio()
        {
            string responseBody = "nada";
            string vv = "nada";
            string v2 = "nada";

            string url = "http://10.10.26.55:443/api/login";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Accept", "application/json");

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string jsonb = "{\"User\":{\"login_id\":\"consultas\",\"password\":\"Consulta1#\"}}";
                streamWriter.Write(jsonb);
            }
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) ;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd().ToString();
                            vv = response.Headers.Get(1).ToString();   //obtencion de los headers
                            //v2 = response.Headers.Get(0).ToString();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                responseBody = ex.ToString();
            }
            return vv;//responseBody+'_'+v2+'_'+vv;
        }

        public string user_bio()
        {
            string url = "http://10.10.26.55:443/api/users?group_id=1&limit=1&offset=1&order_by=user_id%3Afalse&userId=1&last_modified=10009";
            var myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            myRequest.Headers.Add("bs-session-id", token_bio());
            WebResponse response = myRequest.GetResponse();
            Stream strReader = response.GetResponseStream();
            StreamReader objReader = new StreamReader(strReader);
            string responseBody = objReader.ReadToEnd().ToString();
            return responseBody;
        }

        public string event_search_bio()
        {
            string responseBody = "nada";
            string vv = "nada";
            string url = "http://10.10.26.55:443/api/events/search";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Headers.Add("bs-session-id", token_bio());
            request.ContentType = "application/json";
            request.Headers.Add("Accept", "application/json");
            // limite, operador, valor fecha-hora //
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string jsonb = "{\"Query\":{\"limit\":51,\"conditions\":[{\"column\":\"datetime\",\"operator\":3,\"values\":[\"2019-07-30T15:00:00.000Z\"]}],\"orders\":[{\"column\":\"datetime\",\"descending\":false}]}}";
                streamWriter.Write(jsonb);
            }
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) ;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd().ToString();
                            //vv = response.Headers.Get(1).ToString();   //obtencion de los headers
                            //v2 = response.Headers.Get(0).ToString();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                responseBody = ex.ToString();
            }
            return responseBody;
        }


        public string device_bio()
        {
            string responseBody = "nada";
            string vv = "nada";
            string url = "http://10.10.26.55:443/api/devices?monitoring_permission=false";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("bs-session-id", token_bio());
            request.ContentType = "application/json";
            request.Headers.Add("Accept", "application/json");
            WebResponse response = request.GetResponse();
            Stream strReader = response.GetResponseStream();
            StreamReader objReader = new StreamReader(strReader);
            responseBody = objReader.ReadToEnd().ToString();
            return responseBody;
        }


        public string Checando_tok(string p_id, string fechahora, string dispositivoId, string posi, string tk_beat)
        {
            //string token = giveme_tok();
            //string direccion = "https: //api.workbeat.com/v2/asi/checada?id=8251&fechahora=2019-11-05T09:04:08&dispositivoId=11948&dirección=[E|1|N]";
            string direccion = "https://api.workbeat.com/v2/asi/checada?id=" + p_id + "&fechahora=" + fechahora + "&dispositivoId=" + dispositivoId + "&dirección=" + posi;
            string responseBody = "nada";
            var url = direccion;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + tk_beat);
            request.Headers.Add("Accept", "application/json");

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) ;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            Console.WriteLine(responseBody);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                responseBody = ex.ToString();
            }
            return responseBody;
        }

        public string[] bio_event_search(string tk_bio) //Tuple<string, string> bio_event_search() 
        {
            string responseBody = "nada";
            string vv = "nada";
            string cadena = "nada";
            string url = "http://10.10.26.55:443/api/events/search";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";


            //request.Headers.Add("bs-session-id", token_bio());
            request.Headers.Add("bs-session-id", tk_bio);


            request.ContentType = "application/json";
            request.Headers.Add("Accept", "application/json");
            // limite, operador, valor fecha-hora //
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                //string jsonb = "{\"Query\":{\"limit\":51,\"conditions\":[{\"column\":\"datetime\",\"operator\":1,\"values\":[\"2019-07-30T15:00:00.000Z\"]}],\"orders\":[{\"column\":\"datetime\",\"descending\":false}]}}";
                string jsonb = "{\"Query\":{\"limit\":51,\"orders\":[{\"column\":\"datetime\",\"descending\":true}]}}";
                streamWriter.Write(jsonb);
            }
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) ;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            //Data_event.raiz_data from_js = JsonSerializer.Deserialize<Data_event.raiz_data>(objReader.ReadToEnd().ToString());
                            responseBody = objReader.ReadToEnd().ToString();

                            //vv = response.Headers.Get(1).ToString();   //obtencion de los headers
                            //v2 = response.Headers.Get(0).ToString();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex);
                responseBody = ex.ToString();
            }
            Data_event.raiz_data from_js = JsonSerializer.Deserialize<Data_event.raiz_data>(responseBody);
            Data_event.intern_row from_js_2 = JsonSerializer.Deserialize<Data_event.intern_row>(from_js.EventCollection);
            /////////////////////////////////////////
            String str = from_js_2.rows.ToString();
            //dynamic objects = JsonSerializer.Deserialize<dynamic>(str);
            dynamic objects = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(str);
            string[] respuestas = new string[objects.Count];
            int ct = 0;
            for (int i_el = 0; i_el < objects.Count; i_el++)
            {
                Data_event.intern_data from_js_3 = JsonSerializer.Deserialize<Data_event.intern_data>(objects[i_el].ToString());
                Data_event.intern_in_data from_js_dtime = JsonSerializer.Deserialize<Data_event.intern_in_data>(from_js_3.device_id);

                if (from_js_3.user_id_name != null)
                {
                    //respuestas[ct] = "fecha_:" + from_js_3.datetime.ToString() + "__Id_dispositivo_:" + from_js_dtime.id.ToString() + "__Nombre_:" + from_js_dtime.name.ToString() + '_' + from_js_3.user_id_name.ToString();
                    respuestas[ct] = from_js_3.datetime.ToString() + "/" + from_js_dtime.id.ToString() + "/" + from_js_dtime.name.ToString() + '/' + from_js_3.user_id_name.ToString();
                    ct += 1;
                    //Data_event.intern_in_data from_js_usr = JsonSerializer.Deserialize<Data_event.intern_in_data>(from_js_3.user_id_name);
                    //cadena = cadena + "fecha_:" + from_js_3.datetime.ToString() + "__Id_dispositivo_:" + from_js_dtime.id.ToString() + "__Nombre_:" + from_js_dtime.name.ToString()+'_'+ from_js_3.user_id_name.ToString();// +'_'+ from_js_dtime.user_id_name.ToString();// + '_' + usd_id.user_id.ToString() + '_' + usd_id.name.ToString();
                }
            }
            string[] salida = new string[ct - 1];
            Array.Copy(respuestas, 0, salida, 0, ct - 1);
            return salida;// +'_'+usd_id.user_id.ToString()+'_'+usd_id.name.ToString();      //objects[0].ToString(); //from_js_2.rows.ToString() + objects[0];// from_js_dtime.ToString() + '_' + from_js_dvid.id.ToString() + '_' + from_js_dvid.name.ToString();//from_js_2.rows.ToString();//from_js.EventCollection.ToString();//responseBody.ToString(); // from_js.ToString();//new Tuple<string, string>(from_js.ToString(), from_js.ToString());  //responseBody;
        }


        /// <summary>
        /// componente de registro desde Biostar a workBeat
        /// </summary>
        public string[] registrando_bio_beat()
        {
            string to_check = " ";
            string reponse = " ";
            string tok_bio = token_bio();
            string tok_beat = getToken();
            //"2021-06-17T17:13:30Z"/"542194755"/"COMEDOR 2 BARRA MEXICANA"/"1794(MARTINEZ HERRERA ARTURO)"
            string[] data_check = bio_event_search(tok_bio);

            string[] salida = new string[data_check.Length];
            //string[] salida = new string[data_check.Length];

            if (data_check.Length == 0)
            {
                string p_id1 = "8251";
                string fechahora = "2019-11-05T09:04:08";
                string dispositivoId = "11948";
                string posi = "[E|1|N]";
                salida[0] = Checando_tok(p_id1, fechahora, dispositivoId, posi, tok_beat);
            }
            else
            {

                for (int i_ch = 0; i_ch < data_check.Length; i_ch++)
                {
                    to_check = data_check[i_ch];
                    string[] to_check_words = to_check.Split('/');
                    string[] p_idin = to_check_words[3].Split('"');

                    //string to_fix = to_check_words[0].ToString().Split('"')[1];
                    string ppp = to_check_words[0].ToString().Split('"')[1].Split('Z')[0];

                    // test
                    //53462021-06-17T18:54:25542194757[E|1|N]
                    //82512019-11-05T09:04:0811948[E|1|N]

                    string p_id = "8251";
                    string fechahora = "2019-11-05T09:04:08";
                    string dispositivoId = "11948";
                    string posi = "[E|1|N]";

                    //string p_id = p_idin[1].Split('(')[0];
                    //string fechahora = ppp;
                    //string dispositivoId = to_check_words[1].ToString().Split('"')[1];
                    //string posi = to_check_words[2].ToString().Split('"')[1]; //"[E|1|N]";

                    //salida[i_ch] = "Dato Biostar rechazado:_" + p_i + fecha + dispositivo + pos + "________________Dato en Workbeat aceptado:_" + p_id1 + fechahora + dispositivoId + posi;// to_fix;//p_id[1].Split('(')[0];
                    //salida[i_ch] = p_id1 + fechahora + dispositivoId + "[E|1|N]";// to_fix;//p_id[1].Split('(')[0];

                    //salida[i_ch] = Checando(p_id1,fechahora,dispositivoId,posi,tok_beat);   ////////////// datos Aceptados /////////////
                    salida[i_ch] = Checando_tok(p_id, fechahora, dispositivoId, posi, tok_beat);
                }
            }
            //Array.Copy(respuestas, 0, salida, 0, ct - 1);
            return salida;
        }*/


    }
}
