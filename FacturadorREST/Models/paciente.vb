Public Class paciente
    Public Property pers_Codigo As Long
    Public Property pers_Apellido As String
    Public Property pers_Nombre As String
    Public Property pers_FechaNacimiento As Date
    Public Property pers_Sexo As String
    Public Property pers_Pais As String
    Public Property prov_Codigo As Long
    Public Property pers_Calle As String
    Public Property pers_Altura As String
    Public Property pers_Piso As String
    Public Property pers_Depto As String
    Public Property pers_Localidad As String
    Public Property pers_CodigoPostal As String
    Public Property pers_Mail As String
    Public Property pers_Cuit As String
    Public Property tiva_Codigo As Integer
    Public Property tper_Codigo As Integer
    Public Property tdoc_Codigo As Integer
    Public Property pers_NumeroDocumento As String
    Public Property COD_CLIENT As String
    Public Property pers_FechaActualizacion As Date
    Public Property pais_Codigo As Long
    Public Property tper_Descripcion As String
    Public Property tiva_Descripcion As String
    Public Property tdoc_Descripcion As String
    Public Property paci_HistoriaClinica As String
    Public Property test_Codigo As Integer
    Public Property paci_FechaGeneracion As Date
    Public Property paci_ApellidoFacturacion As String
    Public Property paci_NombreFacturacion As String
    Public Property paci_CUITFacturacion As String
    Public Property paci_DomicilioFacturacion As String
    Public Property tiva_CodigoEntidad As Integer
    Public Property tiva_DescripcionEntidad As String
    Public Property paci_CodigoNumerico As Long
    Public Property coberturas As List(Of pacienteCobertura)
End Class

Public Class pacienteCobertura
    Public Property paci_CodigoNumerico As Long
    Public Property pers_Codigo As Long
    Public Property cobe_Codigo As Long
    Public Property cobe_Descripcion As String
    Public Property pers_Apellido As String
    Public Property pers_Nombre As String
    Public Property pers_CUIT As String
    Public Property plan_Codigo As Long
    Public Property plan_CodigoInterno As String
    Public Property plan_Descripcion As String
    Public Property tcon_Codigo As Long
    Public Property tcon_Descripcion As String
    Public Property ivap_Porcentaje As Double
    Public Property paco_Afiliado As String
    Public Property tcob_Codigo As Integer
    Public Property tcob_Descripcion As String
    Public Property tcob_CodigoInterno As String
End Class

Public Class respuestaPaciente
    Public Property estado As String
    Public Property mensaje As String
    Public Property ok As Boolean
    Public Property paciente As paciente
End Class

Public Class respuestaPacientesUnificado
    Public Property estado As String
    Public Property mensaje As String
    Public Property ok As Boolean
    Public Property pacientes As List(Of paciente)
End Class

Public Class respuestaPacienteCoberturas
    Public Property estado As String
    Public Property mensaje As String
    Public Property ok As Boolean
    Public Property coberturas As List(Of pacienteCobertura)
End Class

Public Class pacienteTotem
    Public Property Id As Long
    Public Property Apellido As String
    Public Property Nombre As String
    Public Property Calle As String
    Public Property Altura As String
    Public Property Piso As String
    Public Property Depto As String
    Public Property Sexo As String
    Public Property FechaNacimiento As Date
    Public Property Edad As Integer
    Public Property TipoDocumento As String
    Public Property NroDocumento As String
    Public Property Pais As String
    Public Property Provincia As String
    Public Property Localidad As String
    Public Property CodigoPostal As String
    Public Property NroHistoriaClinica As String
    Public Property NroHistoriaClinicaAnt As String
    Public Property Telefonos As String
    Public Property Coberturas As String
    Public Property Mail As String
    Public Property LocalidadCodigo As String
    Public Property PaisCodigo As String
    Public Property ProvinciaCodigo As String
    Public Property TipoDocumentoCodigo As String
    Public Property TipoDocumentoAFIP As String
    Public Property Fallecido As Boolean
    Public Property FechaFallecido As Date
    Public Property TipoDocumentoPersonaCargo As String
    Public Property NroDocumentoPersonaCargo As String
    Public Property EstadoIdentificacion As String
    Public Property AustralSalud As Boolean
End Class

Public Class estadoRespuestaTotem
    Public Property CodigoRespuesta As Integer
    Public Property Mensaje As String
End Class

Public Class respuestaPacienteTotem
    Public Property EstadoRespuesta As estadoRespuestaTotem
    Public Property Pacientes As List(Of pacienteTotem)
End Class

Public Class requestPacienteTotem
    Public Property NroDocumento As String
End Class