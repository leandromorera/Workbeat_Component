﻿using K2_Betterware_Workbeat_Assistance.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace K2_Betterware_Workbeat_Assistance.Core.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<Assistance>> GetAssistance();
        string getToken();
        string get_employed(string stp);
        string get_empleado(string stp);
        string get_persona(string stp, string npr);

        string checando(string id, string fechora, string disid, string direccion);

        /*string Token_bio();
        string User_bio();
        string Event_search_bio();
        string Device_bio();

        string Checando_tok(string id, string fechora, string disid, string direccion, string tk_beat);
        string[] bio_event_search(string tk_bio);
        string[] registrando_bio_beat();*/

    }
}
