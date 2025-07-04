
$(document).ready(function () {
    InitializeFormularios();
});

function InitializeFormularios() {
    console.log("Formularios initialized");

    // Inicializar validaciones en tiempo real
    InitializeValidations();

    // Configurar evento de envío del formulario
    $('#financialEntityForm').on('submit', function (e) {
        e.preventDefault();
        SubmitFinancialEntity();
    });

    // Configurar validaciones en tiempo real
    SetupRealTimeValidations();
}

function InitializeValidations() {
    // Validación de solo números para campos específicos
    $('#legalId, #phone').on('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    // Validación de código bancario (alfanumérico)
    $('#bankCode').on('input', function () {
        this.value = this.value.replace(/[^A-Za-z0-9]/g, '');
    });
}

function SetupRealTimeValidations() {
    // Validación de cédula jurídica
    $('#legalId').on('blur', function () {
        ValidateLegalId($(this));
    });

    // Validación de código bancario
    $('#bankCode').on('blur', function () {
        ValidateBankCode($(this));
    });

    // Validación de nombre
    $('#name').on('blur', function () {
        ValidateName($(this));
    });

    // Validación de teléfono
    $('#phone').on('blur', function () {
        ValidatePhone($(this));
    });

    // Validación de email
    $('#email').on('blur', function () {
        ValidateEmail($(this));
    });
}

function ValidateLegalId($input) {
    const value = $input.val().trim();
    const isValid = /^[0-9]{12}$/.test(value);

    if (!value) {
        SetFieldError($input, "La cédula jurídica es obligatoria.");
        return false;
    } else if (!isValid) {
        SetFieldError($input, "Debe contener 12 dígitos numéricos.");
        return false;
    } else {
        SetFieldSuccess($input);
        return true;
    }
}

function ValidateBankCode($input) {
    const value = $input.val().trim();
    const isValid = /^[A-Za-z0-9]+$/.test(value);

    if (!value) {
        SetFieldError($input, "El código bancario es obligatorio.");
        return false;
    } else if (!isValid) {
        SetFieldError($input, "Solo se permiten letras y números.");
        return false;
    } else {
        SetFieldSuccess($input);
        return true;
    }
}

function ValidateName($input) {
    const value = $input.val().trim();

    if (!value) {
        SetFieldError($input, "El nombre es obligatorio.");
        return false;
    } else if (value.length > 100) {
        SetFieldError($input, "No debe exceder los 100 caracteres.");
        return false;
    } else {
        SetFieldSuccess($input);
        return true;
    }
}

function ValidatePhone($input) {
    const value = $input.val().trim();
    const isValid = /^[0-9]{8}$/.test(value);

    if (!value) {
        SetFieldError($input, "El teléfono es obligatorio.");
        return false;
    } else if (!isValid) {
        SetFieldError($input, "Debe tener 8 dígitos numéricos.");
        return false;
    } else {
        SetFieldSuccess($input);
        return true;
    }
}

function ValidateEmail($input) {
    const value = $input.val().trim();
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!value) {
        SetFieldError($input, "El correo electrónico es obligatorio.");
        return false;
    } else if (!emailRegex.test(value)) {
        SetFieldError($input, "Ingrese un correo válido.");
        return false;
    } else {
        SetFieldSuccess($input);
        return true;
    }
}

function ValidateLocation() {
    const latitude = $('#latitude').val();
    const longitude = $('#longitude').val();

    if (!latitude || !longitude) {
        SetLocationError("Debe seleccionar una ubicación.");
        return false;
    } else {
        ClearLocationError();
        return true;
    }
}

function SetFieldError($input, message) {
    $input.removeClass('is-valid').addClass('is-invalid');
    $input.siblings('.invalid-feedback').text(message).show();
}

function SetFieldSuccess($input) {
    $input.removeClass('is-invalid').addClass('is-valid');
    $input.siblings('.invalid-feedback').hide();
}

function SetLocationError(message) {
    $('#mapContainer').css('border-color', '#DA291C');
    $('#mapContainer').siblings('.invalid-feedback').text(message).show();
}

function ClearLocationError() {
    $('#mapContainer').css('border-color', '#D9D9D9');
    $('#mapContainer').siblings('.invalid-feedback').hide();
}

function ValidateForm() {
    let isValid = true;

    // Validar todos los campos
    isValid &= ValidateLegalId($('#legalId'));
    isValid &= ValidateBankCode($('#bankCode'));
    isValid &= ValidateName($('#name'));
    isValid &= ValidatePhone($('#phone'));
    isValid &= ValidateEmail($('#email'));
    isValid &= ValidateLocation();

    return Boolean(isValid);
}

function SubmitFinancialEntity() {
    // Ocultar mensajes previos
    HideMessages();

    // Validar formulario
    if (!ValidateForm()) {
        ShowError("Por favor corrija los errores antes de enviar.");
        return;
    }

    // Mostrar loading
    ShowLoading(true);

    // Preparar datos
    const formData = {
        legalId: $('#legalId').val().trim(),
        bankCode: $('#bankCode').val().trim(),
        name: $('#name').val().trim(),
        phone: $('#phone').val().trim(),
        email: $('#email').val().trim(),
        latitude: parseFloat($('#latitude').val()),
        longitude: parseFloat($('#longitude').val())
    };

    console.log("Enviando datos:", formData);

    // Realizar petición AJAX al WebAPI
    $.ajax({
        url: '/api/FinancialEntity/Create',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            console.log("Respuesta exitosa:", response);
            ShowSuccess();
            ResetForm();
        },
        error: function (xhr, status, error) {
            console.error("Error:", xhr.responseText);

            let errorMessage = "Error al procesar la solicitud.";

            if (xhr.responseJSON && xhr.responseJSON.error) {
                errorMessage = xhr.responseJSON.error;
            } else if (xhr.responseText) {
                try {
                    const errorObj = JSON.parse(xhr.responseText);
                    errorMessage = errorObj.error || errorObj.message || errorMessage;
                } catch (e) {
                    errorMessage = "Error del servidor.";
                }
            }

            ShowError(errorMessage);
        },
        complete: function () {
            ShowLoading(false);
        }
    });
}

function ShowSuccess() {
    $('#successMessage').removeClass('d-none').addClass('d-block');
    $('#errorMessage').removeClass('d-block').addClass('d-none');

    // Scroll al mensaje
    $('html, body').animate({
        scrollTop: $('#successMessage').offset().top - 100
    }, 500);
}

function ShowError(message) {
    $('#errorText').text(message);
    $('#errorMessage').removeClass('d-none').addClass('d-block');
    $('#successMessage').removeClass('d-block').addClass('d-none');

    // Scroll al mensaje
    $('html, body').animate({
        scrollTop: $('#errorMessage').offset().top - 100
    }, 500);
}

function HideMessages() {
    $('#successMessage').removeClass('d-block').addClass('d-none');
    $('#errorMessage').removeClass('d-block').addClass('d-none');
}

function ShowLoading(show) {
    const $submitBtn = $('#submitBtn');

    if (show) {
        $submitBtn.prop('disabled', true);
        $submitBtn.html('<span class="spinner-border spinner-border-sm me-2" role="status"></span>Enviando...');
    } else {
        $submitBtn.prop('disabled', false);
        $submitBtn.html('<i class="fas fa-paper-plane me-2"></i>Enviar Formulario');
    }
}

function ResetForm() {
    // Limpiar formulario
    $('#financialEntityForm')[0].reset();

    // Limpiar validaciones visuales
    $('.is-valid, .is-invalid').removeClass('is-valid is-invalid');
    $('.invalid-feedback').hide();

    // Limpiar mapa
    ResetMapContainer();

    // Limpiar coordenadas hidden
    $('#latitude, #longitude').val('');
}

function ResetMapContainer() {
    $('#mapContainer').html(`
        <div class="text-center text-muted">
            <i class="fas fa-map-marker-alt fa-3x mb-2" style="color: #DA291C;"></i>
            <div>Haga clic para seleccionar ubicación</div>
            <small>Seleccione la ubicación de su oficina principal</small>
        </div>
    `);

    $('#coordinatesDisplay').removeClass('d-block').addClass('d-none');
    ClearLocationError();
}

// =============================================
// FUNCIONES DE MAPA (Simulación)
// =============================================

function OpenLocationPicker() {
    // Simulación de selección de ubicación
    // En una implementación real, esto abriría un mapa interactivo

    console.log("Abriendo selector de ubicación...");

    // Simular coordenadas de San José, Costa Rica con variación aleatoria
    const lat = 9.9281 + (Math.random() - 0.5) * 0.01;
    const lng = -84.0907 + (Math.random() - 0.5) * 0.01;

    SetLocation(lat, lng);
}

function SetLocation(lat, lng) {
    // Guardar coordenadas
    $('#latitude').val(lat.toFixed(6));
    $('#longitude').val(lng.toFixed(6));

    // Actualizar display
    $('#latitudeDisplay').text(lat.toFixed(6));
    $('#longitudeDisplay').text(lng.toFixed(6));

    // Mostrar coordenadas
    $('#coordinatesDisplay').removeClass('d-none').addClass('d-block');

    // Actualizar contenedor del mapa
    $('#mapContainer').html(`
        <div class="text-center">
            <i class="fas fa-map-marker-alt fa-3x mb-2" style="color: #007A3D;"></i>
            <div class="text-success fw-bold">Ubicación seleccionada</div>
            <small class="text-muted">Haga clic para cambiar la ubicación</small>
        </div>
    `);

    // Limpiar error de ubicación
    ClearLocationError();

    console.log(`Ubicación establecida: ${lat.toFixed(6)}, ${lng.toFixed(6)}`);
}