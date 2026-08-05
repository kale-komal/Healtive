import PageHeader from "@/components/super-admin/PageHeader";
import HospitalForm from "@/components/super-admin/hospital/HospitalForm";

export default function CreateHospitalPage() {

    return (

        <>

            <PageHeader
                title="Create Hospital"
                subtitle="Register a new hospital into the system."
            />

            <HospitalForm />

        </>

    );

}