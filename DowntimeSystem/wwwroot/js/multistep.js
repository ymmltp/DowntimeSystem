$(
    current = 0,
    //设置模态框的状态
    _step = function(){
        let howLong=$(".modal-steps ").actual("width")/3;
        console.log(howLong)
        $('.step-main').css("height",getMaxHeight());
        $('.step-slide').eq(current).fadeIn(500)
        $('.step').eq(current).addClass("current");

        let preventResponse=false;
        $('#next').click(function(){
            if(!preventResponse){
                if(current!=($('.step-slide').length-1)){
            
                    $('.step').eq(current).addClass("completed");
                
                    current++;
                    
                    let timeOut=null;
                    clearTimeout(timeOut);
                    timeOut=setTimeout(function(){
                        console.log(current)
                        $('.step').eq(current).addClass("current");
                    },200);

                    console.log(current);
                    $('.step-slide').eq(current).stop().fadeIn(500).siblings().fadeOut(500);
                    if(current==($('.step-slide').length-1)){
                        $("#finish").removeClass("noneDeslpay");
                        $(this).addClass("noneDeslpay");
                    }else{
                        $("#finish").addClass("noneDeslpay");
                        $(this).removeClass("noneDeslpay");
                    }
                    if(current==0){
                        $('#pre').addClass('unVisible')
                    }else{
                        $('#pre').removeClass('unVisible')
                    }
                }
                
                // $(".step").eq(0).css("width",current*howLong);
                preventResponse=true;
                setTimeout(function(){
                    preventResponse=false;
                },500);
            }
        })
        $('#pre').click(function(){
            if(!preventResponse){
                if(current!=0){
                    $('.step').eq(current).removeClass("current");
                    current--;
                    let timeOut=null;
                    clearTimeout(timeOut);
                    timeOut=setTimeout(function(){
                        $('.step').eq(current).removeClass("completed");
                    },200);
                    $('.step-slide').eq(current).stop().fadeIn(500).siblings().fadeOut(500);
                    if(current==($('.step-slide').length-1)){
                        $("#finish").removeClass("noneDeslpay");
                        $("#next").addClass("noneDeslpay");
                    }else{
                        $("#finish").addClass("noneDeslpay");
                        $("#next").removeClass("noneDeslpay");
                    }
                    if(current==0){
                        $(this).addClass('unVisible')
                    }else{
                        $(this).removeClass('unVisible')
                    }
                }    
            
                preventResponse=true;
                setTimeout(function(){
                    preventResponse=false;
                },500);
            }
        })
        $('#finish').click(function () {
            //上传数据
            if (checkFormNoNull("#rseqidContent")) {
                var data = $("#tmptable").bootstrapTable('getData');
                postDataWithArray(BasicURL+"/api/EqSparepartChange/Add_EqResourceLists", data)
                    .then(res => {
                        showSuccess("添加成功");
                        //关闭模态框
                        $("#NewModal").modal('hide');
                    })
                    .catch(err => {
                        showError(err);
                    });
            }
        })
    },
    //重置模态框状态
    inimultistep = function (){
        $('.step').each(function(){
            $(this).removeClass("completed");
            $(this).removeClass("current");
        });
        current = 0;
        $('.step').eq(current).addClass("current");
        $('.step-slide').eq(current).stop().fadeIn(500).siblings().fadeOut(500);
        $('#pre').addClass('unVisible')
        $("#finish").addClass("noneDeslpay");
        $("#next").removeClass("noneDeslpay");
        $("#tmptable").bootstrapTable('destroy')
    },
)

function getMaxHeight(){
    //let MaxHeight=0;
    //$('.step-slide').each(function(){
    //    let actualHeight=parseFloat($(this).actual('outerHeight'));
    //    if(actualHeight>MaxHeight){
    //        MaxHeight=actualHeight
    //    }
    //})
    return 400;
}