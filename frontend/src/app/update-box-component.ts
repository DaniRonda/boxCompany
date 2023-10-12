﻿import {Component} from "@angular/core";
import {FormBuilder, Validators} from "@angular/forms";
import {Box, ResponseDto} from "../models";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {environment} from "../environments/environment";
import {State} from "../state";
import {firstValueFrom} from "rxjs";
import {ModalController, ToastController} from "@ionic/angular";

@Component({
  template: `

  <ion-list>
    <ion-item>
      <ion-input [formControl]="updateNewboxForm.controls.boxName" data-testid="titleInput" label="Insert box name">

      </ion-input>
      <div *ngIf="!updateNewboxForm.controls.boxName.valid">The name of the box has to be longer</div>
    </ion-item>
    <ion-item>
      <ion-input [formControl]="updateNewboxForm.controls.boxDescription" data-testid="boxDescriptionInput"  label="Insert a description">
      </ion-input>
    </ion-item>
    <ion-item>
      <ion-input  [formControl]="updateNewboxForm.controls.boxImgUrl"  data-testid="boxImgUrlInput"   label="Insert an image">

      </ion-input>
    </ion-item>

    <ion-item>
      <ion-button data-testid="submit" [disabled]="updateNewboxForm.invalid" (click)="submit()">send</ion-button>
    </ion-item>
  </ion-list>

  `
})
export class UpdateBoxComponent {

  updateNewboxForm = this.fb.group({
    boxName: ['', Validators.minLength(7)],
    boxDescription: ['', Validators.required],
    boxImgUrl: ['', Validators.required]
  })

  constructor(public fb: FormBuilder, public modalController: ModalController, public http: HttpClient, public state: State, public toastController: ToastController) {
  }

  async submit() {

    try {
      const observable =     this.http.put<ResponseDto<Box>>(environment.baseUrl + '/api/boxes', this.updateNewboxForm.getRawValue())

      const response = await firstValueFrom(observable);
      this.state.boxes.push(response.responseData!);

      const toast = await this.toastController.create({
        message: 'Box succefully updated (hopefully)',
        duration: 1000,
        color: "success"
      })
      toast.present();
      this.modalController.dismiss();
    } catch (e) {
      if(e instanceof HttpErrorResponse) {
        const toast = await this.toastController.create({
          message: e.error.messageToClient,
          color: "danger"
        });
        toast.present();
      }
    }


  }
}
