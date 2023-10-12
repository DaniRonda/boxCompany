export class Box{
  boxName?:string;
  boxDescription?:string;
  boxImgUrl?:string;
  boxId?:number;
}

export class ResponseDto<T>{
  responseData?:T;
  messageToClient?: string;
}
