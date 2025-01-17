$PBExportHeader$w_aes_48.srw
forward
global type w_aes_48 from window
end type
type cb_1 from commandbutton within w_aes_48
end type
type st_3 from statictext within w_aes_48
end type
type st_2 from statictext within w_aes_48
end type
type st_1 from statictext within w_aes_48
end type
type sle_decrypted from singlelineedit within w_aes_48
end type
type sle_encrypted from singlelineedit within w_aes_48
end type
type sle_raw from singlelineedit within w_aes_48
end type
end forward

global type w_aes_48 from window
integer width = 4754
integer height = 1980
boolean titlebar = true
string title = "Untitled"
boolean controlmenu = true
boolean minbox = true
boolean maxbox = true
boolean resizable = true
long backcolor = 67108864
string icon = "AppIcon!"
boolean center = true
cb_1 cb_1
st_3 st_3
st_2 st_2
st_1 st_1
sle_decrypted sle_decrypted
sle_encrypted sle_encrypted
sle_raw sle_raw
end type
global w_aes_48 w_aes_48

on w_aes_48.create
this.cb_1=create cb_1
this.st_3=create st_3
this.st_2=create st_2
this.st_1=create st_1
this.sle_decrypted=create sle_decrypted
this.sle_encrypted=create sle_encrypted
this.sle_raw=create sle_raw
this.Control[]={this.cb_1,&
this.st_3,&
this.st_2,&
this.st_1,&
this.sle_decrypted,&
this.sle_encrypted,&
this.sle_raw}
end on

on w_aes_48.destroy
destroy(this.cb_1)
destroy(this.st_3)
destroy(this.st_2)
destroy(this.st_1)
destroy(this.sle_decrypted)
destroy(this.sle_encrypted)
destroy(this.sle_raw)
end on

type cb_1 from commandbutton within w_aes_48
integer x = 786
integer y = 1072
integer width = 457
integer height = 132
integer taborder = 30
integer textsize = -12
integer weight = 400
fontcharset fontcharset = ansi!
fontpitch fontpitch = variable!
fontfamily fontfamily = swiss!
string facename = "Tahoma"
string text = "execute"
end type

event clicked;string ls_raw, ls_iv, ls_key 

nvo_encryptionhelper l_nvo_encryption_helper
l_nvo_encryption_helper = create nvo_encryptionhelper

ls_raw = sle_raw.text

ls_key = l_nvo_encryption_helper.of_keygenaes256( )

string ls_temp[1 to 2]
ls_temp = l_nvo_encryption_helper.of_encryptaescbcwithiv( ls_raw, ls_key)
ls_iv = ls_temp[2]
sle_encrypted.text = ls_temp[1]

sle_decrypted.text = l_nvo_encryption_helper.of_decryptaescbcwithiv( sle_encrypted.text, ls_key, ls_iv)
end event

type st_3 from statictext within w_aes_48
integer x = 421
integer y = 860
integer width = 457
integer height = 76
integer textsize = -12
integer weight = 400
fontcharset fontcharset = ansi!
fontpitch fontpitch = variable!
fontfamily fontfamily = swiss!
string facename = "Tahoma"
long textcolor = 33554432
long backcolor = 67108864
string text = "Decrypted"
boolean focusrectangle = false
end type

type st_2 from statictext within w_aes_48
integer x = 421
integer y = 688
integer width = 457
integer height = 76
integer textsize = -12
integer weight = 400
fontcharset fontcharset = ansi!
fontpitch fontpitch = variable!
fontfamily fontfamily = swiss!
string facename = "Tahoma"
long textcolor = 33554432
long backcolor = 67108864
string text = "Encrypted"
boolean focusrectangle = false
end type

type st_1 from statictext within w_aes_48
integer x = 421
integer y = 488
integer width = 457
integer height = 76
integer textsize = -12
integer weight = 400
fontcharset fontcharset = ansi!
fontpitch fontpitch = variable!
fontfamily fontfamily = swiss!
string facename = "Tahoma"
long textcolor = 33554432
long backcolor = 67108864
string text = "Raw"
boolean focusrectangle = false
end type

type sle_decrypted from singlelineedit within w_aes_48
integer x = 942
integer y = 836
integer width = 1179
integer height = 132
integer taborder = 20
integer textsize = -12
integer weight = 400
fontcharset fontcharset = ansi!
fontpitch fontpitch = variable!
fontfamily fontfamily = swiss!
string facename = "Tahoma"
long textcolor = 33554432
borderstyle borderstyle = stylelowered!
end type

type sle_encrypted from singlelineedit within w_aes_48
integer x = 942
integer y = 664
integer width = 2578
integer height = 132
integer taborder = 10
integer textsize = -12
integer weight = 400
fontcharset fontcharset = ansi!
fontpitch fontpitch = variable!
fontfamily fontfamily = swiss!
string facename = "Tahoma"
long textcolor = 33554432
borderstyle borderstyle = stylelowered!
end type

type sle_raw from singlelineedit within w_aes_48
integer x = 942
integer y = 492
integer width = 1193
integer height = 132
integer taborder = 10
integer textsize = -12
integer weight = 400
fontcharset fontcharset = ansi!
fontpitch fontpitch = variable!
fontfamily fontfamily = swiss!
string facename = "Tahoma"
long textcolor = 33554432
borderstyle borderstyle = stylelowered!
end type

