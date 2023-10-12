import {Component, OnInit} from "@angular/core";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {environment} from "../environments/environment";
import {firstValueFrom} from "rxjs";
import {Box, ResponseDto } from "src/models";
import { State } from "src/state";
import {ModalController, ToastController } from "@ionic/angular";
import { CreateBoxComponent } from "./create-box-component";
import { UpdateBoxComponent } from "./update-box-component";

@Component({
  template: `
    <ion-content style="position: absolute; top: 0;">
      <ion-list>
        <ion-card [attr.data-testid]="'card_'+box.boxName" *ngFor="let box of state.boxes">
          <ion-toolbar>
            <ion-title>{{box.boxName}}</ion-title>
          </ion-toolbar>
          <ion-buttons>
            <ion-button (click)="deleteBox(box.boxId)">delete</ion-button>
          </ion-buttons>
          <ion-card-subtitle>{{box.boxDescription}}</ion-card-subtitle>
          <img style="max-height: 200px;" [src]="box.boxImgUrl">
        </ion-card>
      </ion-list>

      <ion-fab>
        <ion-fab-button data-testid="createBox" (click)="openModal()">
          <ion-icon name="add"></ion-icon>
        </ion-fab-button>
      </ion-fab>

      <ion-fab>
        <ion-fab-button data-testid="updateBox" (click)="openModal2()">
          <ion-icon name="create"></ion-icon>
        </ion-fab-button>
      </ion-fab>

    </ion-content>  `,
})

export class BoxFeed implements  OnInit{
  constructor(public http: HttpClient,public modalController: ModalController,
              public state: State, public toastController: ToastController) {

  }

  async fetchBox() {
    const result = await firstValueFrom(this.http.get<ResponseDto<Box[]>>(environment.baseUrl + '/api/boxes'))
    this.state.boxes = result.responseData!;

  }
ngOnInit() {this.fetchBox()
}
  async deleteBox(boxId: number | undefined) {
    try {
      await firstValueFrom(this.http.delete(environment.baseUrl + '/api/boxes/'+boxId))
      this.state.boxes = this.state.boxes.filter(b => b.boxId != boxId)
      const toast = await this.toastController.create({
        message: 'Successfully deleted',
        duration: 1000,
        color: "success"
      })
      toast.present();
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
  async openModal() {
    const modal = await this.modalController.create({
      component: CreateBoxComponent
    });
    modal.present();
  }

  async openModal2() {
    const modal = await this.modalController.create({
      component: UpdateBoxComponent
    });
    modal.present();
  }
}



