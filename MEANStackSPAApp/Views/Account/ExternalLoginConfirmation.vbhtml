@ModelType ExternalLoginConfirmationViewModel
@Code
    ViewBag.Title = "Registrarse"
End Code

<h2>@ViewBag.Title.</h2>
<h3>Asocie su cuenta @ViewBag.LoginProvider.</h3>

@Using Html.BeginForm("ExternalLoginConfirmation", "Account", New With { .ReturnUrl = ViewBag.ReturnUrl }, FormMethod.Post, New With {.class = "form-horizontal", .role = "form"})
    @Html.AntiForgeryToken()

    @<text>
    <h4>Formulario de asociación</h4>
    <hr />
    @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
    <p class="text-info">
        Se autenticó correctamente con <strong>@ViewBag.LoginProvider</strong>.
        Introduzca un nombre de usuario para este sitio y haga clic en el botón Registrar para finalizar
        el inicio de sesión.
    </p>
    <div class="form-group">
        @Html.LabelFor(Function(m) m.Email, New With {.class = "col-md-2 control-label"})
        <div class="col-md-10">
            @Html.TextBoxFor(Function(m) m.Email, New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(m) m.Email, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div class="form-group">
        @Html.LabelFor(Function(m) m.Hometown, New With {.class = "col-md-2 control-label"})
        <div class="col-md-10">
            @Html.TextBoxFor(Function(m) m.Hometown, New With {.class = "form-control"})
        </div>
    </div>    
    <div class="form-group">
        <div class="col-md-offset-2 col-md-10">
            <input type="submit" class="btn btn-default" value="Registrarse" />
        </div>
    </div>
    </text>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
