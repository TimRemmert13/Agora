using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Image
    {
        private const string BASE_URL = "http://localhost:3010/api/";
        private string _storageId;

        public Image(string fileName)
        {
            StorageId = fileName;
            FileName = fileName;
            _uri = BASE_URL + "image/" + _storageId;
        }

        [Key]
        public string StorageId
        {
            get { return _storageId; }
            set
            {
                string[] valueArray = value.Split('.');
                valueArray[0] = Guid.NewGuid().ToString();
                _storageId = String.Join(".", valueArray);
            }
        }
        public string FileName { get; set; }
        private string _uri;
        public string Uri
        {
            get { return _uri; }
            set
            {
                _uri = BASE_URL + "image/" + _storageId;
            }
        }
    }
}