import { Component, OnInit, Inject } from '@angular/core';
import { QLessModel } from '../entities/QLessModel';
import { HttpClient, HttpParams} from '@angular/common/http';
import { QLessViewModel } from '../entities/QLessViewModel';
import { PriceMatrix } from '../entities/PriceMatrix';
import { QLessRegistration } from '../entities/QLessRegistration';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit  {
  public qLessResult: QLessModel;
  public qLessModel: QLessModel;
  public qLessViewModel: QLessViewModel;
  public qlessRegister: QLessRegistration;
  public priceMatrix: PriceMatrix;
  public cardId: string;
  baseUrl: string;
  httpClient: HttpClient;
  public isShow: boolean;
  public isEntry: boolean;
  public stnName: string;
  public isShowRegister: boolean;
  public  txtStatus: string;
    fare: string;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    this.priceMatrix = new PriceMatrix();
    this.qLessModel = new QLessModel();
    this.qLessModel.isEntry = !this.priceMatrix.stationFr;
   
    this.isEntry = !this.priceMatrix.stationFr;
    this.txtStatus = "";
    this.fare = "";
    this.isShowRegister = false;
   }

  scanCard(): void {

    let params = new HttpParams()
      .set("id", this.cardId)
     // .set("isEntry", this.isEntry ? 'true' : 'false')
      .set("stnFrom", (this.priceMatrix.stationFr ? this.priceMatrix.stationFr.toString() : ''))
      .set("line", (this.priceMatrix.stationFr ? this.priceMatrix.line.toString() : ''))
    this.httpClient.get<QLessViewModel>(this.baseUrl + 'qless', { params }).subscribe(result => {

      this.qLessViewModel = result;
      this.priceMatrix = result.priceMatrix;
      this.qLessModel = result.qLessModel;
      this.qLessModel.active = (this.qLessModel.expirationDate >= new Date());
      this.stnName = this.qLessModel.isEntry ? this.priceMatrix.stationFrName : this.priceMatrix.stationToName;
      this.isShow = true;
      if (!this.qLessModel.isEntry) {
        this.isEntry = true; //reset
      } else {
        this.isEntry = false;
      }
     
      //if (this.isEntry) { //reset
      //  this.priceMatrix = null;
      //}
    }, error => console.error(error));
  }
  showRegister(): void {

    this.isShowRegister = true;
  }
  registerCard(): void {
    this.qlessRegister.QLessCardSerialNo = this.qLessModel.serialNo;
    this.qlessRegister.QLessCardId = this.qLessModel.id;
    let params = new HttpParams()
      .set("qLessRegistration", JSON.stringify(this.qlessRegister))
    
    this.httpClient.post<QLessViewModel>(this.baseUrl + 'qless', { params }).subscribe(result => {
    this.txtStatus = "You are now registered, enjoy 20% discount pon your next trip with additional 3% discount on succesding trips (max of 4 trips/day)";
     
    }, error => console.error(error));
  }
}
