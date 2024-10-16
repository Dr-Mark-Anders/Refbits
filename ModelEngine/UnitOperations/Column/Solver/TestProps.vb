﻿Namespace RusselColumn

    Public Class TestProps

        Public Const PHVap = 15968.1498929234   'kJ/mol
        Public Const BHVap = 18814.7425158597

        Public Const PHForm = -103890  'kJ/mol
        Public Const BHForm = -126190
        Public Const BaseT = 298.15

        Dim t, t2, a, b, c, d, e, f, g, h, i, j, delta

        Function VP(ByVal t As Double, ByVal a As Double, ByVal b As Double, ByVal c As Double)
            VP = Math.Exp(a - b / (t + 273.15 + c)) / 750.0615613
        End Function

        Function PropaneVP(ByVal tc As Double) As Double 'kPa
            t = tc + 273.15
            a = 52.3785
            b = -3490.55
            c = 0.0#
            d = -6.10875
            e = 0.0000111869
            f = 2.0#
            g = 0.0#
            h = 0.0#
            i = 0.0#
            j = 0.0#
            PropaneVP = Math.Exp(a + (b / (t + c)) + d * Math.Log(t) + e * (t ^ f))
        End Function

        Function ButaneVP(ByVal tc As Double) As Double 'kPa
            t = tc + 273.15
            a = 66.945
            b = -4604.09
            c = 0
            d = -8.25491
            e = 0.0000115706
            f = 2.0#
            g = 0.0#
            h = 0.0#
            i = 0.0#
            j = 0.0#
            ButaneVP = Math.Exp(a + (b / (t + c)) + d * Math.Log(t) + e * (t ^ f))
        End Function

        Function PropaneVapEnth(ByVal t As Double) As Double ' kJ/kg
            Dim PropaneVapEntha, PropaneVapEnthb
            t = t + 273.15
            t2 = BaseT
            a = 39.4889
            b = 0.395
            c = 0.00211409
            d = 0.000000396486
            e = -0.000000000667176
            f = 0.000000000000167936
            g = 1.0#
            h = 0.0#
            i = 0.0#
            j = 0.0#
            PropaneVapEntha = a + b * t + c * t ^ 2 + d * t ^ 3 + e * t ^ 4 + f * t ^ 5
            PropaneVapEnthb = a + b * t2 + c * t2 ^ 2 + d * t2 ^ 3 + e * t2 ^ 4 + f * t2 ^ 5
            PropaneVapEnth = ((PropaneVapEntha - PropaneVapEnthb) * 44 + PHForm) '/ 1000

        End Function

        Function ButaneVapEnth(ByVal t As Double) As Double ' kJ/kg
            Dim ButaneVapEntha, ButaneVapEnthb
            t = t + 273.15
            t2 = BaseT
            a = 67.721
            b = 0.00854058
            c = 0.00327699
            d = -0.00000110968
            e = 0.000000000176646
            f = -0.00000000000000639926
            g = 1.0#
            h = 0.0#
            i = 0.0#
            j = 0.0#
            ButaneVapEntha = a + b * t + c * t ^ 2 + d * t ^ 3 + e * t ^ 4 + f * t ^ 5
            ButaneVapEnthb = a + b * t2 + c * t2 ^ 2 + d * t2 ^ 3 + e * t2 ^ 4 + f * t2 ^ 5
            ButaneVapEnth = ((ButaneVapEntha - ButaneVapEnthb) * 58.12 + BHForm) '/ 1000
        End Function

        Function PropaneLiqEnth(ByVal t As Double) As Double
            ' kJ/kg
            'PropaneLiqEnth = 2.403 * t * 44
            PropaneLiqEnth = PropaneVapEnth(t) - PHVap '/ 1000
        End Function

        Function ButaneLiqEnth(ByVal t As Double) As Double
            ' kJ/kg
            'ButaneLiqEnth = 2.352 * t * 58.12
            ButaneLiqEnth = ButaneVapEnth(t) - BHVap '/ 1000
        End Function

        Function StreamLiqEnthalpy(ByVal prop As Double, ByVal but As Double, ByVal t As Double) As Double
            StreamLiqEnthalpy = PropaneLiqEnth(t) * prop + ButaneLiqEnth(t) * but
        End Function

        Function StreamVapEnthalpy(ByVal prop As Double, ByVal but As Double, ByVal t As Double) As Double
            StreamVapEnthalpy = PropaneVapEnth(t) * prop + ButaneVapEnth(t) * but
        End Function

    End Class
End Namespace