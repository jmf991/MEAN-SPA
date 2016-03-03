@ModelType SendCodeViewModel
@Code
    ViewBag.Title = "Enviar"
End Code

<h2>@ViewBag.Title.</h2>

@Using Html.BeginForm("SendCode", "Account", New With { .ReturnUrl = Model.ReturnUrl }, FormMethod.Post, New With { .class = "form-horizontal", .role = "form" })
    @Html.AntiForgeryToken()
    @Html.Hidden("rememberMe", Model.RememberMe)
    @<text>
    <h4>Enviar código de verificación</h4>
    <hr />
    <div class="row">
        <div class="col-md-8">
            Seleccione el proveedor de autenticación en dos fases:
            @Html.DropDownListFor(Function(model) model.SelectedProvider, Model.Providers)
            <input type="submit" value="Enviar" class="btn btn-default" />
        </div>
    </div>
    </text>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
