using BusinessLayer.Dtos;
using DataAccessLayer.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Help
{
    public static class Helper
    {
        private static readonly Random _random = new Random();


        public static int GenerateRandomSixDigitNumber()
        {
            return _random.Next(100000, 1000000);// الادني مشمول والاقصي غير مشمول
        }

        public static string? ReturnNullIfEmpty(string? word)
        {
            return string.IsNullOrEmpty(word) ? null : word;
        }

        public static EmailContentDto GeUpdateOrderEmailContect(EnApplicationOrderType enApplicationOrderType, UpdateOrderEmailQueueDto dto)
        {
            var content = new EmailContentDto();

            switch (enApplicationOrderType)
            {
                case EnApplicationOrderType.UnderProcessing: // UnderProcessing
                    content.Subject = $"Order #{dto.ApplicationId} is being processed!";
                    content.Messsage = $"Thank you for your order! We have received your order #{dto.ApplicationId} and our team is currently preparing it for shipment.";
                    content.TextBody = $"Your order #{dto.ApplicationId} is under processing. We are preparing it for shipment.";
                    break;

                case EnApplicationOrderType.Shipped: // Shipped
                    content.Subject = $"Your order #{{dto.OrderId}} has been shipped! 🚚";
                    content.Messsage = $"Great news! Your order #{dto.ApplicationId} is on its way. You can track your shipment using the link: {dto.TrackOrderUrl}";
                    content.TextBody = $"Your order #{dto.ApplicationId} has been shipped and is on its way. Track it here: {dto.TrackOrderUrl}";
                    break;

                case EnApplicationOrderType.Delivered: // Delivered
                    content.Subject = $"Delivered: Order #{dto.ApplicationId} 🎉";
                    content.Messsage = $"Your order #{dto.ApplicationId} has been successfully delivered. We hope you enjoy your purchase!";
                    content.TextBody = $"Your order #{dto.ApplicationId} has been successfully delivered. Thank you for choosing us!";
                    break;

                case EnApplicationOrderType.Canceled: // Canceled
                    content.Subject = $"Canceled: Order #{dto.ApplicationId}";
                    content.Messsage = $"Your order #{dto.ApplicationId} has been successfully canceled. If you have been charged, your refund will be processed soon.";
                    content.TextBody = $"Your order #{dto.ApplicationId} has been canceled.";
                    break;

                default:
                    // حالة احتياطية لأي رقم غير متوقع
                    content.Subject = $"Update regarding your order #{dto.ApplicationId}";
                    content.Messsage = $"There is an update regarding your order #{dto.ApplicationId}. Please check your account for details.";
                    content.TextBody = $"There is an update regarding your order #{dto.ApplicationId}.";
                    break;
            }

            return content;
        }

        public static string GetUpdateOrderEmailImage(EnApplicationOrderType enApplicationOrderType, string Baseurl)
        {
           
            switch (enApplicationOrderType)
            {
                case EnApplicationOrderType.UnderProcessing:
                    return $"{Baseurl}/images/OrderUnderProcessing.png";

                case EnApplicationOrderType.Shipped:
                    return $"{Baseurl}/images/OrderShipped.png";

                case EnApplicationOrderType.Delivered:
                    return $"{Baseurl}/images/OrderDelivered.png";

                case EnApplicationOrderType.Canceled:
                    return $"{Baseurl}/images/OrderCanceled.png";

                    default: return "";
            }
        }
    }
}
