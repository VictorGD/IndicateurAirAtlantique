//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IndicateurAirAtlantique
{
    using System;
    using System.Collections.Generic;
    
    public partial class Avion
    {
        public Avion()
        {
            this.Capacite = new HashSet<Capacite>();
            this.Vol = new HashSet<Vol>();
        }
    
        public int id { get; set; }
        public double conso { get; set; }
    
        public virtual ICollection<Capacite> Capacite { get; set; }
        public virtual ICollection<Vol> Vol { get; set; }
    }
}