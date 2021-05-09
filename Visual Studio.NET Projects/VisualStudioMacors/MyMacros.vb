Imports System
Imports EnvDTE
Imports EnvDTE80
Imports System.Diagnostics

Public Module ReadWriteLocks


    Sub ReadLock()
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "ReaderWriterLock.AcquireReaderLock(10000);"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "try"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "{"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "}"
        DTE.ActiveDocument.Selection.LineUp()
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.LineDown()
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "finally"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "{"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "}"
        DTE.ActiveDocument.Selection.LineUp()
        DTE.ActiveDocument.Selection.NewLine()

        DTE.ActiveDocument.Selection.Text = "ReaderWriterLock.ReleaseReaderLock();"
    End Sub


    Sub WriteLock()
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "OOAdvantech.Synchronization.LockCookie lockCookie=ReaderWriterLock.UpgradeToWriterLock(10000);"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "try"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "{"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "}"
        DTE.ActiveDocument.Selection.LineUp()
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.LineDown()
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "finally"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "{"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "}"
        DTE.ActiveDocument.Selection.LineUp()
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "ReaderWriterLock.DowngradeFromWriterLock(ref lockCookie);"
    End Sub

    Sub ObjectStateTrasition()
        DTE.ActiveDocument.Selection.Cut()
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.LineUp()
        DTE.ActiveDocument.Selection.EndOfLine()
        DTE.ActiveDocument.Selection.Text = "using(Transactions.ObjectStateTransition stateTransition =new Transactions.ObjectStateTransition()"
        DTE.ActiveDocument.Selection.CharLeft()

        DTE.ActiveDocument.Selection.CharRight()
        DTE.ActiveDocument.Selection.Text = ")"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "{"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Paste()
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "stateTransition.Consistent = true;"
        DTE.ActiveDocument.Selection.NewLine()
        DTE.ActiveDocument.Selection.Text = "}"
        DTE.ActiveDocument.Selection.NewLine()
    End Sub

    Sub NextDocumentBookmark()
        DTE.ActiveDocument.Selection.NextBookmark()
    End Sub

    Sub PreviousDocumentBookmark()
        DTE.ActiveDocument.Selection.PreviousBookmark()
    End Sub




End Module
