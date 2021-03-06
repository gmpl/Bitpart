﻿namespace Bitpart.Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open FSharpPlus
open Bitpart.Utils
open Bitpart.Lingo
open Bitpart.Lingo.Pickler
open Bitpart.Multiuser

[<TestClass>]
type Protocol () =
    [<TestMethod>]        
    member __.PickleUnpickle () =
        let msg1 = 
            {
                sender      = "TheSender_ñéè"
                recipients  = ["recípient1";"recípient2";"@AllUsers"]
                subject     = "NewMessage"
                content     = 
                    pickle valueP (
                        LPropList [
                            "move"  , LList [LRect (LInteger 2, LFloat -2879.54, LInteger -7984, LFloat 1.1); LDate (2012,2,28,34786)]
                            "object", LString "Queen"
                            "Color" , LColor (255,0,55)
                            "img"   , LPicture [|12uy;24uy;24uy|]
                                ])
                errorCode   = -1024
                timeStamp   = 2015
            }

        let bytes = pickle   (messageP None) msg1
        let msg2  = unpickle (messageU None) bytes
        Assert.AreEqual (msg1, msg2)

    [<TestMethod>]   
    member this.PickleUnpickleTime () =
        let s = System.DateTime.Now
        iter (fun _ -> this.PickleUnpickle()) {1..40000}
        Assert.IsTrue ((System.DateTime.Now - s).TotalMilliseconds < 2000.)

    [<TestMethod>] 
    member __.ParseToString () =     
        let v1 = 
            LPropList [
                "move"  , LList [LRect (LInteger 2, LFloat -2879.54, LInteger -7984, LFloat 1.1)]
                "object", LString "Queen"
                "Color" , LColor (255, 0, 55)
                ]
        let s = string v1
        let v2 = parse s
        Assert.IsTrue((v1 = v2))
        ()


    [<TestMethod>] 
    member this.ParseToStringTime () =
        let s = System.DateTime.Now
        iter (fun _ -> this.ParseToString ()) {1..40000}
        Assert.IsTrue ((System.DateTime.Now - s).TotalMilliseconds < 3000.)
        ()