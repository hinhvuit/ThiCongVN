<template>
    <div style="padding: 20px;">
        <el-card>
            <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
                <el-tab-pane :label="$t('public.SecurityDynamics')" name="first">
                    <el-form ref="ruleFormRef" :inline="true" status-icon :model="QueryParams" class="demo-ruleForm">
                        <el-form-item prop="title">
                            <el-input v-model="QueryParams.title" :placeholder="$t('public.enterTitle')" />
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" @click="getList(1)">{{$t('public.Query')}}</el-button>
                            <el-button @click="fwresetForm">{{$t('public.Reset')}}</el-button>
                        </el-form-item>
                    </el-form>
                    <el-button type="primary" style="margin-bottom: 10px;" @click="addFile(1)">{{$t('public.Add')}}</el-button>
                    <el-table :data="fwtableData" style="width: 100%">
                        <el-table-column type="index" :label="$t('public.SerialNumber')" width="120" />
                        <el-table-column prop="title" :label="$t('public.Title')" />
                        <el-table-column prop="empNo" :label="$t('public.CompressionDiagram')">
                            <template #default="scope"><el-image style="width: 100px; height: 100px"
                                    :src="scope.row.smallFileUrl" fit="contain" /></template>

                        </el-table-column>
                        <el-table-column prop="showOrder" :label="$t('public.DisplaySequence')" />
                        <el-table-column prop="showTime" :label="$t('public.DisplayTime')" />
                        <el-table-column prop="createTime" :label="$t('public.LastModifier')">
                            <template #default="scope">
                                <span>{{ scope.row.createName + '(' + scope.row.createBy + ')' }}</span>
                            </template>
                        </el-table-column>
                        <el-table-column prop="createTime" :label="$t('public.LastModificationTime')" />
                        <el-table-column prop="address" :label="$t('public.Operating')">
                            <template #default="scope">
                                <el-button type="warning" @click="fwedit(scope.row)">{{$t('public.Edit')}}</el-button>
                                <el-button type="danger" @click="fwdel(scope.row)">{{$t('public.delete')}}</el-button>
                            </template>
                        </el-table-column>
                    </el-table>
                    <pagination v-show="fwtotal > 0" :total="fwtotal" v-model:page="QueryParams.pageNum"
                        v-model:limit="QueryParams.pageSize" style="height: 40px;" @pagination="getList" />

                </el-tab-pane>
                <el-tab-pane :label="$t('public.PolicyDocument')" name="second">
                    <el-form ref="ruleFormRef" :inline="true" status-icon :model="QueryParams" class="demo-ruleForm">
                        <el-form-item prop="title">
                            <el-input v-model="QueryParams.title" :placeholder="$t('public.enterTitle')" />
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" @click="getList(1)">{{$t('public.Query')}}</el-button>
                            <el-button @click="fwresetForm">{{$t('public.Reset')}}</el-button>
                        </el-form-item>
                    </el-form>
                    <el-button type="primary" style="margin-bottom: 10px;" @click="addFile(2)">{{$t('public.Add')}}</el-button>
                    <el-table :data="fwtableData" style="width: 100%">
                        <el-table-column type="index" :label="$t('public.SerialNumber')" width="120" />
                        <el-table-column prop="title" :label="$t('public.Title')" />
                        <el-table-column prop="indexType" :label="$t('public.FileType')" />
                        <el-table-column prop="showOrder" :label="$t('public.DisplaySequence')" />
                        <el-table-column prop="showTime" :label="$t('public.DisplayTime')" />
                        <el-table-column prop="createTime" :label="$t('public.LastModifier')">
                            <template #default="scope">
                                <span>{{ scope.row.createName + '(' + scope.row.createBy + ')' }}</span>
                            </template>
                        </el-table-column>
                        <el-table-column prop="createTime" :label="$t('public.LastModificationTime')" />
                        <el-table-column prop="address" :label="$t('public.Operating')">
                            <template #default="scope">
                                <el-button type="warning" @click="fwedit(scope.row)">{{$t('public.Edit')}}</el-button>
                                <el-button type="danger" @click="fwdel(scope.row)">{{$t('public.delete')}}</el-button>
                            </template>
                        </el-table-column>
                    </el-table>
                    <pagination v-show="fwtotal > 0" :total="fwtotal" v-model:page="QueryParams.pageNum"
                        v-model:limit="QueryParams.pageSize" style="height: 40px;" @pagination="getList" />
                </el-tab-pane>
                <el-tab-pane :label="$t('public.OperationInstruction')" name="three">
                    <el-form ref="ruleFormRef" :inline="true" status-icon :model="QueryParams" class="demo-ruleForm">
                        <el-form-item prop="title">
                            <el-input v-model="QueryParams.title" :placeholder="$t('public.enterTitle')" />
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" @click="getList(1)">{{$t('public.Query')}}</el-button>
                            <el-button @click="fwresetForm">{{$t('public.Reset')}}</el-button>
                        </el-form-item>
                    </el-form>
                    <el-button type="primary" style="margin-bottom: 10px;" @click="addFile(3)">{{$t('public.Add')}}</el-button>
                    <el-table :data="fwtableData" style="width: 100%">
                        <el-table-column type="index" :label="$t('public.SerialNumber')" width="120" />
                        <el-table-column prop="title" :label="$t('public.Title')" />
                        <el-table-column prop="showOrder" :label="$t('public.DisplaySequence')" />
                        <el-table-column prop="showTime" :label="$t('public.DisplayTime')" />
                        <el-table-column prop="createTime" :label="$t('public.LastModifier')">
                            <template #default="scope">
                                <span>{{ scope.row.createName + '(' + scope.row.createBy + ')' }}</span>
                            </template>
                        </el-table-column>
                        <el-table-column prop="createTime" :label="$t('public.LastModificationTime')" />
                        <el-table-column prop="address" :label="$t('public.Operating')">
                            <template #default="scope">
                                <el-button type="warning" @click="fwedit(scope.row)">{{$t('public.Edit')}}</el-button>
                                <el-button type="danger" @click="fwdel(scope.row)">{{$t('public.delete')}}</el-button>
                            </template>
                        </el-table-column>
                    </el-table>
                    <pagination v-show="fwtotal > 0" :total="fwtotal" v-model:page="QueryParams.pageNum"
                        v-model:limit="QueryParams.pageSize" style="height: 40px;" @pagination="getList" />
                </el-tab-pane>
                <el-tab-pane :label="$t('public.normalProblem')" name="four">
                    <el-form ref="ruleFormRef" :inline="true" status-icon :model="QueryParams" class="demo-ruleForm">
                        <el-form-item prop="title">
                            <el-input v-model="QueryParams.title" :placeholder="$t('public.enterTitle')" />
                        </el-form-item>
                        <el-form-item>
                            <el-button type="primary" @click="getList(1)">{{$t('public.Query')}}</el-button>
                            <el-button @click="fwresetForm">{{$t('public.Reset')}}</el-button>
                        </el-form-item>
                    </el-form>
                    <el-button type="primary" style="margin-bottom: 10px;" @click="addFile(4)">{{$t('public.Add')}}</el-button>
                    <el-table :data="fwtableData" style="width: 100%">
                        <el-table-column type="index" :label="$t('public.SerialNumber')" width="120" />
                        <el-table-column prop="title" :label="$t('public.Title')" />
                        <el-table-column prop="showOrder" :label="$t('public.DisplaySequence')" />
                        <el-table-column prop="showTime" :label="$t('public.DisplayTime')" />
                        <el-table-column prop="createTime" :label="$t('public.LastModifier')">
                            <template #default="scope">
                                <span>{{ scope.row.createName + '(' + scope.row.createBy + ')' }}</span>
                            </template>
                        </el-table-column>
                        <el-table-column prop="createTime" :label="$t('public.LastModificationTime')" />
                        <el-table-column prop="address" :label="$t('public.Operating')">
                            <template #default="scope">
                                <el-button type="warning" @click="fwedit(scope.row)">{{$t('public.Edit')}}</el-button>
                                <el-button type="danger" @click="fwdel(scope.row)">{{$t('public.delete')}}</el-button>
                            </template>
                        </el-table-column>
                    </el-table>
                    <pagination v-show="fwtotal > 0" :total="fwtotal" v-model:page="QueryParams.pageNum"
                        v-model:limit="QueryParams.pageSize" style="height: 40px;" @pagination="getList" />
                </el-tab-pane>
            </el-tabs>
        </el-card>
        <el-dialog v-model="dialogVisibles" :title="$t('public.delete')" width="500">
            {{$t('public.DeleteOrNot')}} {{ title }}
            <template #footer>
                <div class="dialog-footer">
                    <el-button @click="dialogVisibles = false">{{$t('public.cancel')}}</el-button>
                    <el-button type="primary" @click="delFun">
                        {{$t('public.sure')}}
                    </el-button>
                </div>
            </template>
        </el-dialog>
    </div>
</template>
<script setup>
import {
    indexFile,
    delindexFile
} from '@/api/index/index';
const activeName = ref('first')
const dialogVisibles = ref(false)
const router = useRouter();
const { proxy } = getCurrentInstance();
const fwtotal = ref(0)
const title = ref("")
const type = ref("")
const id = ref("")
const data = reactive({
    form: {},
    QueryParams: {
        pageNum: 1,
        pageSize: 10,
        title: "",
        type: "",
    },
    fwtableData: [],
    options: [],
});
const { QueryParams, fwtableData, options } = toRefs(data);
const handleClick = (tab, event) => {
    switch (tab.props.name) {
        case "first":
            getList(1);
            type.value = 1;
            break;
        case "second":
            getList(2);
            type.value = 2;
            break;
        case "three":
            getList(3);
            type.value = 3;
            break;
        case "four":
            getList(4);
            type.value = 4;
            break;
    }
}


const onSubmit = () => {
    console.log('submit!')
}
function getList(type) {
    if (type == undefined) {
        QueryParams.value.type = 1;
    } else {
        QueryParams.value.type = type;
    }

    indexFile(proxy.addDateRange(QueryParams.value)).then((res) => {
        if (res.code != 200) return proxy.$modal.msgError(res.msg);
        fwtableData.value = res.data.records;
        fwtotal.value = res.data.total;
    })
}
function addFile(type) {
    router.push(`/homeindex/notice/upfile?type=${type}&label=`+proxy.$t('public.Add'));
}
function fwresetForm() {
    QueryParams.value.title = "";
    if (type.value == "") {
        getList(1);
    } else {
        getList(type.value);
    }
}
function fwedit(res) {
    router.push(`/homeindex/notice/upfile?data=${JSON.stringify(res)}&type=${res.type}&label=`+proxy.$t('public.Edit'));
}
function fwdel(res) {
    dialogVisibles.value = true
    title.value = res.title;
    id.value = res.id;
}
function delFun() {
    delindexFile(id.value).then((res) => {
        if (res.code != 200) return proxy.$modal.msgError(res.msg);
        proxy.$modal.msgSuccess(res.msg);
        if (type.value == "") {
            getList(1);
        } else {
            getList(type.value);
        }

        dialogVisibles.value = false;
    })
}
getList();
</script>
<style lang='scss' scoped>
.demo-tabs>.el-tabs__content {
    padding: 32px;
    color: #6b778c;
    font-size: 32px;
    font-weight: 600;
}
</style>