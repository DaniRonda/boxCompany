import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { BoxFeed } from './box-feed';
import {HttpClientModule} from "@angular/common/http";
import { CreateBoxComponent } from './create-box-component';
import {ReactiveFormsModule} from "@angular/forms";
import { UpdateBoxComponent } from './update-box-component';

@NgModule({
  declarations: [AppComponent, BoxFeed, CreateBoxComponent, UpdateBoxComponent],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, HttpClientModule,ReactiveFormsModule],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }],
  bootstrap: [AppComponent],
})
export class AppModule {}
