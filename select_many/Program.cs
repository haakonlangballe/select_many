using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace select_many
{
    class Program
    {
        static void Main(string[] args)
        {
            js();
        }

        static void js()
        {
            string one = @"{
                            'id':'10',
                            'name':'Leslie',
                            'friends':[
                                    {'Id': '1'},
                                    {'Id': '2'},
                                    {'Id': '3'}] }";
            string one_emails = @"{
                            'id':'10',
                            'name':'Leslie',
                            'friends':[
                                    {'Id': '1'},
                                    {'Id': '2'},
                                    {'Id': '3'}],
                            'emails': [
                                    {'adress': 'Leslie@gmail.com'},
                                    {'adress': 'Leslie@hotmail.com'}]}";


            string many = @"[{'id':'10','name':'Leslie','friends':[{'id':1},{'id':2},{'id':3}]}
                            ,{'id':'11','name':'Liliana','friends':[{'id':4}]}
                            ,{'id':'12','name':'Francesca','friends':[]}]";
            string many_mails = @"[{'id':'10','name':'Leslie','friends':[{'id':1},{'id':2},{'id':3}],'emails':[{'adress': 'Leslie@gmail.com'},{'adress': 'Leslie@hotmail.com'}]}
                            ,{'id':'11','name':'Liliana','friends':[{'id':4}],'emails':[{'adress': 'Liliana@gmail.com'}]}
                            ,{'id':'12','name':'Francesca','friends':[],'emails':[]}]";


            var deser = Newtonsoft.Json.JsonConvert.DeserializeObject<people>(one);

            List<people> deser2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<people>>(many);

            var res = deser2.SelectMany(f => f.Friends.Select(p => new flat_person
            {
                Id = f.Id,
                Name = f.Name,
                FriendId = p.Id
            }));

            var res2 = deser2.SelectMany(f => f.Friends.DefaultIfEmpty(new friend()), (p, f) => new flat_person
            {
                Id = p.Id
                ,Name = p.Name
                ,FriendId = f.Id //?? "No friends"
            });

            var deser3 = JsonConvert.DeserializeObject<people>(one_emails);
            var deser4 = JsonConvert.DeserializeObject<List<people>>(many_mails);

            var res3 = deser4.SelectMany(f => f.Friends.DefaultIfEmpty(new friend()), (a, b, c) => new flat_person
            {
                Id = a.Id
                ,Name = a.Name
                ,FriendId = b.Id //?? "No friends"
                ,Email = a.Emails.Count().ToString()
            });

            //List<string> strs = new List<string>();
            //strs.Add("aaa");
            //strs.Add("bbb");
            //strs.Add("ccc");


            //var res2 = strs.SelectMany(s => s.

        }
    }

    public class flat_person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FriendId { get; set; }
        public string Email { get; set; }
    }

    public class friend
    {
        public friend()
        {
            Id = "tom";
        }
        public string Id { get; set; }
    }

    public class email
    {
        public email() { Adress = "none"; }
        public string Adress { get; set; }
    }
    public class people
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<friend> Friends { get; set; }
        public List<email> Emails { get; set; }
    }
    public class peoples
    {
        public List<people> Ppl { get; set; }
    }

    public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
    }
}
