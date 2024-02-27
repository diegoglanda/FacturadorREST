Public Class cobertura
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
    Public Property test_CodigoCobertura As Integer
    Public Property test_CodigoPlan As Integer
End Class

Public Class respuestaCoberturas
    Public Property estado As String
    Public Property mensaje As String
    Public Property ok As Boolean
    Public Property coberturas As List(Of cobertura)
End Class

