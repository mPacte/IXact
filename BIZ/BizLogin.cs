using System;
using System.Collections.Generic;
using DAO;
using EntityModele.Criteres;

namespace BIZ
{
    public class BizLogin
    {
        private DaoLogin daoLogin { get; set; }

        public BizLogin()
        {
            daoLogin = new DaoLogin();
        }

        public void Nouveau(string username, DateTime dateLogin, string adresseip)
        {
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@username", Value = username},
                        new Filtre { Name = "@dateLogin", Value = dateLogin},
                        new Filtre { Name = "@adresseIp", Value = adresseip},
                });
            daoLogin.PsInsert(Params);
        }
    }
}