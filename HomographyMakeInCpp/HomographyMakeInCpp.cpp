// HomographyMakeInCpp.cpp : このファイルには 'main' 関数が含まれています。プログラム実行の開始と終了がそこで行われます。
//

#include "pch.h"
#include <iostream>

using namespace cv;
using std::vector;

void translate(const Mat& homo,double x,double y)
{
    Mat vec(3, 1, CV_64FC1);
    double* ptr = reinterpret_cast<double*>(vec.data);
    ptr[0] = x;
    ptr[1] = y;
    ptr[2] = 1;
    Mat ans = homo * vec;
    ptr = reinterpret_cast<double*>(ans.data);
    std::cout << "translate x is" << ptr[0] / ptr[2] << std::endl;
    std::cout << "translate y is" << ptr[1] / ptr[2] << std::endl;
    std::cout << "vec" << std::endl;
    std::cout << ans << std::endl;
}


void test1()
{
    vector<Point> srcVec(4);
    vector<Point> dstVec(4);

    Point2d s0(0, 0);
    Point2d s1(100, 0);
    Point2d s2(100, 100);
    Point2d s3(0, 100);

    srcVec.push_back(s0);
    srcVec.push_back(s1);
    srcVec.push_back(s2);
    srcVec.push_back(s3);

    Point2d d0(0, 0);
    Point2d d1(500, 0);
    Point2d d2(500, 200);
    Point2d d3(0, 200);

    dstVec.push_back(d0);
    dstVec.push_back(d1);
    dstVec.push_back(d2);
    dstVec.push_back(d3);

    Mat homographyMat = findHomography(srcVec, dstVec, 0);
    std::cout << "=====test1=====" << std::endl;
    std::cout << homographyMat << std::endl;
    translate(homographyMat, 100, 100);
}


void test2()
{
    vector<Point> srcVec(4);
    vector<Point> dstVec(4);

    Point s0(-152, 394);
    Point s1(218, 521);
    Point s2(223, -331);
    Point s3(-163, -219);

    srcVec.push_back(s0);
    srcVec.push_back(s1);
    srcVec.push_back(s2);
    srcVec.push_back(s3);

    Point d0(-666, 431);
    Point d1(500, 300);
    Point d2(480, -308);
    Point d3(-580, -280);

    dstVec.push_back(d0);
    dstVec.push_back(d1);
    dstVec.push_back(d2);
    dstVec.push_back(d3);

    Mat homographyMat = findHomography(srcVec, dstVec, 0);
    std::cout << "=====test2=====" << std::endl;
    std::cout << homographyMat << std::endl;
    translate(homographyMat, (-152.0 + 218.0) / 2.0, (394 + 521) / 2.0);
}


void test3()
{
    vector<Point> srcVec(4);
    vector<Point> dstVec(4);

    Point2d s0(5, 280);
    Point2d s1(301, 256);
    Point2d s2(224, 41);
    Point2d s3(10, 30);

    srcVec.push_back(s0);
    srcVec.push_back(s1);
    srcVec.push_back(s2);
    srcVec.push_back(s3);

    Point2d d0(309, 555);
    Point2d d1(623, 733);
    Point2d d2(543, 49);
    Point2d d3(333, 111);

    dstVec.push_back(d0);
    dstVec.push_back(d1);
    dstVec.push_back(d2);
    dstVec.push_back(d3);

    Mat homographyMat = findHomography(srcVec, dstVec, 0);
    std::cout << "=====test3=====" << std::endl;
    std::cout << homographyMat << std::endl;
    translate(homographyMat, 10, 30);

}

int main()
{
    test1();
    test2();
    test3();
}

// プログラムの実行: Ctrl + F5 または [デバッグ] > [デバッグなしで開始] メニュー
// プログラムのデバッグ: F5 または [デバッグ] > [デバッグの開始] メニュー

// 作業を開始するためのヒント: 
//    1. ソリューション エクスプローラー ウィンドウを使用してファイルを追加/管理します 
//   2. チーム エクスプローラー ウィンドウを使用してソース管理に接続します
//   3. 出力ウィンドウを使用して、ビルド出力とその他のメッセージを表示します
//   4. エラー一覧ウィンドウを使用してエラーを表示します
//   5. [プロジェクト] > [新しい項目の追加] と移動して新しいコード ファイルを作成するか、[プロジェクト] > [既存の項目の追加] と移動して既存のコード ファイルをプロジェクトに追加します
//   6. 後ほどこのプロジェクトを再び開く場合、[ファイル] > [開く] > [プロジェクト] と移動して .sln ファイルを選択します
