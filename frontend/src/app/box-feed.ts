import {Component} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "../environments/environment";
import {firstValueFrom} from "rxjs";

@Component({
  template:'<ion-card> <ion-toolbar><ion-title> </ion-title>  </ion-toolbar></ion-card>',
})
export class BoxFeed{
constructor(public http: HttpClient) {
}

async fetchBox() {
  const result = await firstValueFrom(this.http.get(environment.baseUrl + '/api/boxes'))
}

}
