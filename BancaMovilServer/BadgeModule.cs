using BancaMovilServer.DbModels;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Responses;

namespace BancaMovilServer
{
    public class BadgeModule : Nancy.NancyModule
    {
        public BadgeModule() : base("/Badges")
        {
            // http://localhost:8088/Badges/99
            //getbyid
            Get["/{id}"] = parameter => { return GetById(parameter.id); };

            //add
            Post["/"] = parameter => { return this.AddBadge(); };

            //update
            Put["/{id}"] = parameter => { return this.UpdateBadge(parameter.id); };

            //delete
            Delete["/{id}"] = parameter => { return this.DeleteBadge(parameter.id); };

            //Index
            Get["/"] = parameter => { return this.GetAll(); };

            //Search
            Get["/search/{param}"] = parameter => { return this.Search(parameter.param); };
        }

        private Response Search(string param)
        {
            try
            {
                BadgeContext ctx = new BadgeContext();
                List<Badge> badge = ctx.SearchByName(param);

                Nancy.Response response = new Nancy.Responses.JsonResponse<List<Badge>>(badge, new DefaultJsonSerializer());
                response.StatusCode = HttpStatusCode.OK;
                // uri
                string uri = this.Request.Url.SiteBase + this.Request.Path + "/";
                response.Headers["Location"] = uri;

                return response;

            }
            catch (Exception e)
            {
                String operation = String.Format("BadgesModule.SearchByName({0})", (param == "") ? "No Model Data" : param.ToString());

                var HandleException = (Response)operation;
                HandleException.StatusCode = HttpStatusCode.InternalServerError;
                return HandleException;
            }

        }

        private Response DeleteBadge(int id)
        {
            Badge badge = null;
            try
            {
                badge = this.Bind<Badge>();

                BadgeContext ctx = new BadgeContext();
                ctx.delete(badge);

                // 201 - created
                Nancy.Response response = new Nancy.Responses.JsonResponse<Badge>(badge, new DefaultJsonSerializer());
                response.StatusCode = HttpStatusCode.OK;
                // uri
                string uri = this.Request.Url.SiteBase + this.Request.Path + "/" + badge.Id.ToString();
                response.Headers["Location"] = uri;

                return response;

            }
            catch (Exception e)
            {
                String operation = String.Format("BadgesModule.DeleteBadge({0})", (badge == null) ? "No Model Data" : badge.Title);

                var HandleException = (Response)operation;
                HandleException.StatusCode = HttpStatusCode.InternalServerError;
                return HandleException;
            }

        }

        private Response UpdateBadge(int id)
        {
            Badge badge = null;
            try
            {
                badge = this.Bind<Badge>();

                BadgeContext ctx = new BadgeContext();
                ctx.update(badge);

                // 201 - created
                Nancy.Response response = new Nancy.Responses.JsonResponse<Badge>(badge, new DefaultJsonSerializer());
                response.StatusCode = HttpStatusCode.OK;
                // uri
                string uri = this.Request.Url.SiteBase + this.Request.Path + "/" + badge.Id.ToString();
                response.Headers["Location"] = uri;

                return response;

            }
            catch (Exception e)
            {
                String operation = String.Format("BadgesModule.UpdateBadge({0})", (badge == null) ? "No Model Data" : badge.Title);

                var HandleException = (Response)operation;
                HandleException.StatusCode = HttpStatusCode.InternalServerError;
                return HandleException;
            }
        }

        private Response AddBadge()
        {
            Badge badge = null;
            try
            {
                // bind the request body to the object via a Nancy module.
                badge = this.Bind<Badge>();

                // check exists. Return 409 if it does
                if (badge.Id > 0)
                {
                    string errorMessage = String.Format("Use PUT to update an existing Badge with Id = {0}", badge.Id);

                    var ErrorResponse = (Response)errorMessage;
                    ErrorResponse.StatusCode = HttpStatusCode.Conflict;

                    return  ErrorResponse;
                }

                BadgeContext ctx = new BadgeContext();
                ctx.Add(badge);

                // 201 - created
                Nancy.Response response = new Nancy.Responses.JsonResponse<Badge>(badge, new DefaultJsonSerializer());
                response.StatusCode = HttpStatusCode.Created;
                // uri
                string uri = this.Request.Url.SiteBase + this.Request.Path + "/" + badge.Id.ToString();
                response.Headers["Location"] = uri;

                return response;
            }
            catch (Exception e)
            {
                String operation = String.Format("BadgesModule.AddBadge({0})", (badge == null) ? "No Model Data" : badge.Title);

                var HandleException = (Response)operation;
                HandleException.StatusCode = HttpStatusCode.InternalServerError;
                return HandleException;
            }
        }

        private Response GetById(int id)
        {
            try
            {
                BadgeContext ctx = new BadgeContext();
                Badge badge = ctx.GetById(id);

                Nancy.Response response = new Nancy.Responses.JsonResponse<Badge>(badge, new DefaultJsonSerializer());
                response.StatusCode = HttpStatusCode.OK;
                // uri
                string uri = this.Request.Url.SiteBase + this.Request.Path + "/" + badge.Id.ToString();
                response.Headers["Location"] = uri;

                return response;

            }
            catch (Exception e)
            {
                String operation = String.Format("BadgesModule.GetById({0})", (id == 0) ? "No Model Data" : id.ToString());

                var HandleException = (Response)operation;
                HandleException.StatusCode = HttpStatusCode.InternalServerError;
                return HandleException;
            }
            
        }

        private Response GetAll()
        {
            try
            {
                BadgeContext ctx = new BadgeContext();
                List<Badge> badges = ctx.GetAll();
                
                Nancy.Response response = new Nancy.Responses.JsonResponse<List<Badge>>(badges, new DefaultJsonSerializer());
                response.StatusCode = HttpStatusCode.OK;
                // uri
                string uri = this.Request.Url.SiteBase + this.Request.Path + "/";
                response.Headers["Location"] = uri;

                return response;

            }
            catch (Exception e)
            {
                String operation = String.Format("BadgesModule.GetAll()");

                var HandleException = (Response)operation;
                HandleException.StatusCode = HttpStatusCode.InternalServerError;
                return HandleException;
            }

        }
    }
}
