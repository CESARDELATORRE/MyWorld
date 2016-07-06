﻿using System.Threading.Tasks;

namespace MyWorld.Client.Core.Services
{
    public interface IBaseRequest
    {
        string UrlPrefix { get; set; }

        Task DeleteAsync(string url);
        Task PostAsync(string url);
        Task PostAsync<T>(string url, T entity);
        Task<T> PostAsync<T, U>(string url, U entity);
        Task PutAsync<T>(string url, T entity);
    }
}